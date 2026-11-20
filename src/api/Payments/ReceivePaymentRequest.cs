
using OnePlatform.Clearing.Application.Payments.Commands;
using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.ValueObjects;

using MessageIdVo = OnePlatform.Clearing.Domain.ValueObjects.MessageId;
using EndToEndIdVo = OnePlatform.Clearing.Domain.ValueObjects.EndToEndId;
using CurrencyVo = OnePlatform.Clearing.Domain.ValueObjects.Currency;

namespace OnePlatform.Api.Payments;

public sealed record ReceivePaymentRequest(
    string MessageId,
    string EndToEndId,
    long Amount,
    string Currency,

    // Debtor
    string DebtorAccountNumber,
    string DebtorName,
    Guid DebtorInstitutionId,

    // Creditor
    string CreditorAccountNumber,
    string CreditorName,
    Guid CreditorInstitutionId,

    // Institutional participants
    IReadOnlyCollection<ParticipantRequest> Participants,

    DateTimeOffset InitiatedAt
)
{
    public Result<ReceivePaymentCommand> ToCommand()
    {
        var msgIdResult = MessageIdVo.Create(MessageId);

        if (msgIdResult.IsFailure)
            return Result.Failure<ReceivePaymentCommand>(
                msgIdResult.Error);

        var e2eIdResult = EndToEndIdVo.Create(EndToEndId);

        if (e2eIdResult.IsFailure)
            return Result.Failure<ReceivePaymentCommand>(
                e2eIdResult.Error);

        var currencyResult = CurrencyVo.Create(Currency);
        if (currencyResult.IsFailure)
            return Result.Failure<ReceivePaymentCommand>(
                currencyResult.Error);

        var amountResult = Money.Create(Amount, currencyResult.Value);

        if (amountResult.IsFailure)
            return Result.Failure<ReceivePaymentCommand>(
                amountResult.Error);

        var debtorResult = PaymentParty.Create(
            DebtorAccountNumber,
            DebtorName,
            DebtorInstitutionId
        );

        if (debtorResult.IsFailure)
            return Result.Failure<ReceivePaymentCommand>(
                debtorResult.Error);

        var creditorResult = PaymentParty.Create(
            CreditorAccountNumber,
            CreditorName,
            CreditorInstitutionId
        );

        if (creditorResult.IsFailure)
            return Result.Failure<ReceivePaymentCommand>(
                creditorResult.Error);

        if (Participants is null)
            return Result.Failure<ReceivePaymentCommand>(
                PaymentRequestErrors.ParticipantsRequired);

        var participantInputs = new List<ParticipantInput>();

        foreach (var participant in Participants)
        {
            var institutionIdResult = InstitutionId.Create(participant.InstitutionId);

            if (institutionIdResult.IsFailure)
                return Result.Failure<ReceivePaymentCommand>(
                    institutionIdResult.Error);

            var accountIdResult = AccountId.Create(participant.AccountId);

            if (accountIdResult.IsFailure)
                return Result.Failure<ReceivePaymentCommand>(
                    accountIdResult.Error);

            if (!Enum.IsDefined(participant.Role))
                return Result.Failure<ReceivePaymentCommand>(
                    PaymentRequestErrors.InvalidParticipantRole);

            participantInputs.Add(
                new ParticipantInput(
                    institutionIdResult.Value,
                    accountIdResult.Value,
                    participant.Role
                )
            );
        }

        var command = new ReceivePaymentCommand(
            msgIdResult.Value,
            e2eIdResult.Value,
            amountResult.Value,
            debtorResult.Value,
            creditorResult.Value,
            participantInputs,
            InitiatedAt
        );

        return Result.Success(command);
    }
}

public static class PaymentRequestErrors
{
    public static readonly Error ParticipantsRequired = new(
        "PaymentRequest.ParticipantsRequired",
        "Participants are required."
    );

    public static readonly Error InvalidParticipant = new(
        "PaymentRequest.InvalidParticipant",
        "A participant cannot be null."
    );

    public static readonly Error InvalidParticipantRole = new(
        "PaymentRequest.InvalidParticipantRole",
        "The participant role is invalid."
    );
}
