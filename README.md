# OnePlatform

**A learning project exploring inter-institution payment processing, settlement ledgers, and Domain-Driven Design with .NET.**

OnePlatform is a prototype of a payment infrastructure platform for credit unions. It models how participating financial institutions can exchange payment instructions and settle funds between their settlement accounts.

The project focuses on the correctness of financial operations, explicit domain boundaries, and reliable payment processing. It is being developed incrementally, starting with internal payments between participating institutions.

## Goals

- Explore the business concepts behind payment clearing and settlement.
- Model payment processing using Domain-Driven Design (DDD).
- Implement a settlement ledger with balanced, auditable entries.
- Apply software engineering practices to financial systems, including idempotency, transactional consistency, and automated testing.
- Establish a foundation that can later be extended to external payment rails.

## Domain Overview

A payment system can involve several distinct capabilities:

| Capability | Responsibility |
|---|---|
| Payment execution | Initiates and processes payment instructions. |
| Clearing | Validates and determines how payment obligations are handled between participants. |
| Settlement | Transfers value between participating institutions' settlement accounts. |
| Post-settlement | Handles delivery acknowledgements, payment status, returns, and reconciliation. |
| General ledger | Records the organization's official accounting transactions. |
| Monitoring and alerting | Provides visibility into processing, failures, and operational health. |

These are conceptual capabilities; they do not necessarily correspond to separate applications or deployable services.

## OnePlatform's Role

OnePlatform sits between participating credit unions and provides an internal settlement mechanism for eligible transactions.

Two payment scenarios are considered:

### Scenario 1 — On-us payments

Both the payer's credit union and the payee's credit union are OnePlatform members.

The payment can be settled internally by transferring value between their OnePlatform settlement accounts. No Federal Reserve settlement leg is required for this scenario.

### Scenario 2 — External payments

The payer's credit union is a OnePlatform member, but the payee's institution is not.

This scenario requires integration with an external payment rail and additional clearing and settlement coordination. It is part of the longer-term domain model, not the initial implementation scope.

## Initial Scope

The first iteration focuses exclusively on **on-us payments between two member credit unions**.

In scope:

- Model CU A and CU B as participating institutions.
- Maintain their membership and settlement account information.
- Accept and validate payment instructions.
- Determine whether a payment qualifies for internal routing.
- Settle a payment through balanced ledger entries.
- Track payment status and delivery acknowledgements.
- Simulate the participating credit unions.
- Test payment processing, duplicate submissions, and settlement consistency.

Out of scope for the initial iteration:

- Federal Reserve and FedNow integration.
- Payments to nonmember institutions.
- Real bank connectivity and movement of actual funds.
- Production-grade compliance integrations.
- Multi-currency and net settlement.
- A complete administrative dashboard.

## Architecture

The project uses Domain-Driven Design to organize the software around business capabilities and explicit domain boundaries.

The initial conceptual bounded contexts are:

### Payment Origination
Responsible for receiving payment instructions, validating requests, and determining eligibility for processing.

### Settlement
Responsible for executing internal settlement, enforcing ledger invariants, and maintaining auditable accounting entries.

### Post-Settlement
Responsible for payment notifications, delivery acknowledgements, lifecycle status, returns, and reconciliation.

The initial implementation is intended to use a modular monolith. These boundaries are logical separation within the application, not a commitment to microservices.

### Design Principles

- **Domain-centered modeling:** Business rules belong in the domain model rather than being scattered across controllers.
- **Explicit boundaries:** Each bounded context owns its domain responsibilities.
- **Balanced ledger:** Every successful on-us settlement produces corresponding debit and credit entries.
- **Atomicity:** Both ledger entries must commit together or not at all.
- **Idempotency:** Retrying a payment submission or settlement operation must not create duplicate financial postings.
- **Auditable state:** Payment transitions and ledger entries should be traceable.
- **Separation of concerns:** API endpoints, application use cases, domain logic, and infrastructure have distinct responsibilities.

## Settlement Model

For an on-us payment of $50 from CU A to CU B, OnePlatform records two corresponding ledger entries:

| Settlement account | Entry | Amount |
|---|---|---:|
| CU A | Debit | $50.00 |
| CU B | Credit | $50.00 |
| **Net movement** | | **$0.00** |

The ledger represents the settlement balances between OnePlatform and its member institutions. It does not directly debit or credit the credit unions' retail members' accounts.

Settlement balances are derived from ledger entries rather than treated as independently maintained financial truth.

A successful settlement and the subsequent delivery of funds to the payee are separate events. The receiving credit union is responsible for validating and crediting its own member's account.

## Technology Stack

The initial technology direction includes:

- **Language:** C#
- **Platform:** .NET / ASP.NET Core
- **API:** ASP.NET Core controllers and OpenAPI
- **Architecture:** Domain-Driven Design and modular monolith
- **Testing:** Automated unit and integration tests
- **Persistence:** To be selected and implemented as part of the initial vertical slice
- **Observability:** Structured logs, traces, metrics, and alerts, introduced incrementally

The technology stack will evolve as the project progresses.

## Development Roadmap

1. **Domain foundations** — Model payment instructions, membership, money, and settlement accounts.
2. **Payment ingestion** — Implement request handling, validation, and duplicate detection.
3. **Internal routing** — Resolve member institutions and route eligible payments.
4. **Settlement ledger** — Implement atomic, balanced, idempotent postings.
5. **Payment lifecycle** — Simulate delivery acknowledgements, payment status, and returns.
6. **Testing and observability** — Add integration tests, tracing, operational metrics, and reconciliation.
7. **External settlement** — Explore the nonmember scenario and integration with an external payment rail.

## Project Status

**Early development — initial on-us payment flow.**

The domain and architecture are being developed incrementally. Features listed in the roadmap are planned capabilities, not necessarily implemented functionality.

## Learning Objectives

This project serves as a practical environment for exploring:

- Domain-Driven Design and bounded contexts.
- Aggregate boundaries and domain invariants.
- Financial ledger modeling and accounting consistency.
- Transactional behavior and idempotent processing.
- API design and ISO 20022 payment concepts.
- Distributed systems patterns and reliable message delivery.
- Testing and observability for financial applications.

## Disclaimer

OnePlatform is an educational prototype. It is not connected to financial institutions, does not move real funds, and is not intended for production payment processing. The domain model simplifies real-world payment schemes, regulatory requirements, and settlement arrangements.

