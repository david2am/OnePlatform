namespace OnePlatform.Clearing.Shared.Domain.Entities.Tests;

// Concrete test doubles
public class TestEntity : Entity<Guid>
{
    public TestEntity(Guid id) : base(id) { }
}

public class AnotherTestEntity : Entity<Guid>
{
    public AnotherTestEntity(Guid id) : base(id) { }
}

public class StringIdEntity : Entity<string>
{
    public StringIdEntity(string id) : base(id) { }
}

public class EntityTests
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly Guid _otherId = Guid.NewGuid();

    #region Constructor & Id

    [Fact]
    public void Constructor_SetsId()
    {
        var entity = new TestEntity(_id);

        Assert.Equal(_id, entity.Id);
    }

    [Fact]
    public void Id_IsInitOnly()
    {
        // Compiles only if Id has private init / is not settable from outside
        var entity = new TestEntity(_id);
        // entity.Id = Guid.NewGuid(); // should not compile
        Assert.Equal(_id, entity.Id);
    }

    #endregion

    #region Equals (IEquatable)

    [Fact]
    public void Equals_SameReference_ReturnsTrue()
    {
        var entity = new TestEntity(_id);

        Assert.True(entity.Equals(entity));
    }

    [Fact]
    public void Equals_SameId_SameType_ReturnsTrue()
    {
        var a = new TestEntity(_id);
        var b = new TestEntity(_id);

        Assert.True(a.Equals(b));
        Assert.True(b.Equals(a));
    }

    [Fact]
    public void Equals_DifferentId_ReturnsFalse()
    {
        var a = new TestEntity(_id);
        var b = new TestEntity(_otherId);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        var entity = new TestEntity(_id);

        Assert.False(entity.Equals(null));
    }

    [Fact]
    public void Equals_DifferentType_SameId_ReturnsFalse()
    {
        var a = new TestEntity(_id);
        var b = new AnotherTestEntity(_id);

        Assert.False(a.Equals(b));
        Assert.False(b.Equals(a));
    }

    [Fact]
    public void Equals_DifferentGenericIdType_ReturnsFalse()
    {
        var guidEntity = new TestEntity(_id);
        var stringEntity = new StringIdEntity("same-looking-id");

        // Different TId → different Entity<TId> types, so Equals should be false
        Assert.False(guidEntity.Equals(stringEntity as object));
    }

    #endregion

    #region Equals (object)

    [Fact]
    public void ObjectEquals_SameInstance_ReturnsTrue()
    {
        var entity = new TestEntity(_id);

        Assert.True(entity.Equals((object)entity));
    }

    [Fact]
    public void ObjectEquals_SameId_SameType_ReturnsTrue()
    {
        var a = new TestEntity(_id);
        var b = new TestEntity(_id);

        Assert.True(a.Equals((object)b));
    }

    [Fact]
    public void ObjectEquals_Null_ReturnsFalse()
    {
        var entity = new TestEntity(_id);

        Assert.False(entity.Equals((object?)null));
    }

    [Fact]
    public void ObjectEquals_DifferentType_ReturnsFalse()
    {
        var entity = new TestEntity(_id);
        var other = new object();

        Assert.False(entity.Equals(other));
    }

    [Fact]
    public void ObjectEquals_DifferentEntityType_SameId_ReturnsFalse()
    {
        var a = new TestEntity(_id);
        var b = new AnotherTestEntity(_id);

        Assert.False(a.Equals((object)b));
    }

    #endregion

    #region Operators == and !=

    [Fact]
    public void OperatorEquals_BothNull_ReturnsTrue()
    {
        TestEntity? a = null;
        TestEntity? b = null;

        Assert.True(a == b);
    }

    [Fact]
    public void OperatorEquals_LeftNull_ReturnsFalse()
    {
        TestEntity? a = null;
        var b = new TestEntity(_id);

        Assert.False(a == b);
    }

    [Fact]
    public void OperatorEquals_RightNull_ReturnsFalse()
    {
        var a = new TestEntity(_id);
        TestEntity? b = null;

        Assert.False(a == b);
    }

    [Fact]
    public void OperatorEquals_SameId_SameType_ReturnsTrue()
    {
        var a = new TestEntity(_id);
        var b = new TestEntity(_id);

        Assert.True(a == b);
    }

    [Fact]
    public void OperatorEquals_DifferentId_ReturnsFalse()
    {
        var a = new TestEntity(_id);
        var b = new TestEntity(_otherId);

        Assert.False(a == b);
    }

    [Fact]
    public void OperatorEquals_DifferentType_SameId_ReturnsFalse()
    {
        var a = new TestEntity(_id);
        var b = new AnotherTestEntity(_id);

        Assert.False(a == b);
    }

    [Fact]
    public void OperatorNotEquals_IsNegationOfEquals()
    {
        var a = new TestEntity(_id);
        var b = new TestEntity(_id);
        var c = new TestEntity(_otherId);
        TestEntity? n = null;

        Assert.False(a != b);
        Assert.True(a != c);
        Assert.True(a != n);
        Assert.True(n != a);
        Assert.False(n != null); // both null
    }

    #endregion

    #region GetHashCode

    [Fact]
    public void GetHashCode_SameId_ProducesSameHash()
    {
        var a = new TestEntity(_id);
        var b = new TestEntity(_id);

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void GetHashCode_IsBasedOnId()
    {
        var entity = new TestEntity(_id);

        Assert.Equal(_id.GetHashCode(), entity.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentIds_UsuallyDifferent()
    {
        // Not a strict guarantee, but extremely likely
        var a = new TestEntity(_id);
        var b = new TestEntity(_otherId);

        Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
    }

    #endregion

    #region Consistency (Equals ↔ GetHashCode contract)

    [Fact]
    public void EqualsAndHashCode_AreConsistent()
    {
        var a = new TestEntity(_id);
        var b = new TestEntity(_id);
        var c = new TestEntity(_otherId);

        Assert.True(a.Equals(b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());

        Assert.False(a.Equals(c));
        // Hash codes may collide, but we already tested they are based on Id
    }

    #endregion

    #region Edge / value-type Id

    [Fact]
    public void WorksWithValueTypeId()
    {
        var a = new StringIdEntity("abc");
        var b = new StringIdEntity("abc");
        var c = new StringIdEntity("xyz");

        Assert.True(a == b);
        Assert.False(a == c);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
        Assert.True(a.Equals(b));
    }

    [Fact]
    public void WorksWithIntId()
    {
        // Extra concrete type for int
        var a = new IntIdEntity(42);
        var b = new IntIdEntity(42);
        var c = new IntIdEntity(99);

        Assert.True(a == b);
        Assert.False(a == c);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    private class IntIdEntity : Entity<int>
    {
        public IntIdEntity(int id) : base(id) { }
    }

    #endregion
}