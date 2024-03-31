// based on https://github.com/mcintyre321/ValueOf

using System.Linq.Expressions;
using System.Reflection;

namespace StackAPI.Utils;
public class ValueOf<TValue, TClass> where TClass : ValueOf<TValue, TClass>, new() {
  private static readonly Func<TClass> Factory;

  protected virtual void Validate() { }

  static ValueOf() {
    ConstructorInfo ctor = typeof(TClass)
      .GetTypeInfo()
      .DeclaredConstructors
      .First();

    var argsExp = new Expression[0];
    NewExpression newExp = Expression.New(ctor, argsExp);
    LambdaExpression lambda = Expression.Lambda(typeof(Func<TClass>), newExp);

    Factory = (Func<TClass>)lambda.Compile();
  }

  protected virtual bool Equals(ValueOf<TValue, TClass> other) {
    return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
  }

  public override bool Equals(object? obj) {
    return obj != null
      && (ReferenceEquals(this, obj) || obj.GetType() == GetType()
      && Equals((ValueOf<TValue, TClass>)obj));
  }

  public override int GetHashCode() {
    return EqualityComparer<TValue>.Default.GetHashCode(Value!);
  }

  public static bool operator ==(ValueOf<TValue, TClass> a, ValueOf<TValue, TClass> b) {
    return a is null && b is null || (a is not null && b is not null && a.Equals(b));
  }

  public static bool operator !=(ValueOf<TValue, TClass> a, ValueOf<TValue, TClass> b) {
    return !(a == b);
  }

  public override string ToString() {
    return Value is not null ? Value!.ToString()! : "";
  }

  public static TClass From(TValue value) {
    TClass c = Factory();
    c.Value = value;
    c.Validate();

    return c;
  }
  public TValue Value { get; protected set; } = default!;
}
