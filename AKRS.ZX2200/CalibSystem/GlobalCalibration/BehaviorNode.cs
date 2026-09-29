using System;
using System.Collections.Generic;
using System.Linq;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration;

public abstract class BehaviorNode
{
    public abstract BehaviorStatus Tick(Blackboard blackboard);
}


public enum BehaviorStatus { Success, Failure, Running }

public class Blackboard
{
    private Dictionary<string, object> data = new();

    public void Set<T>(string key, T value)
    {
        this.data[key] = value!;
    }

    public T Get<T>(string key)
    {
        return this.data.TryGetValue(key, out object v) ? (T)v : default!;
    }

    public bool Has(string key)
    {
        return this.data.ContainsKey(key);
    }
}

public class SequenceNode : BehaviorNode
{
    private readonly List<BehaviorNode> children;
    private int current = 0;

    public SequenceNode(params BehaviorNode[] children)
    {
        this.children = children.ToList();
    }

    public override BehaviorStatus Tick(Blackboard blackboard)
    {
        while (this.current < this.children.Count)
        {
            BehaviorStatus status = this.children[this.current].Tick(blackboard);
            if (status == BehaviorStatus.Running)
            {
                return BehaviorStatus.Running;
            }

            if (status == BehaviorStatus.Failure)
            {
                this.current = 0;
                return BehaviorStatus.Failure;
            }

            this.current++;
        }

        this.current = 0;
        return BehaviorStatus.Success;
    }
}

public class SelectorNode : BehaviorNode
{
    private readonly List<BehaviorNode> children;
    private int current = 0;

    public SelectorNode(params BehaviorNode[] children)
    {
        this.children = children.ToList();
    }

    public override BehaviorStatus Tick(Blackboard blackboard)
    {
        while (this.current < this.children.Count)
        {
            BehaviorStatus status = this.children[this.current].Tick(blackboard);
            if (status == BehaviorStatus.Running)
            {
                return BehaviorStatus.Running;
            }

            if (status == BehaviorStatus.Success)
            {
                this.current = 0;
                return BehaviorStatus.Success;
            }

            this.current++;
        }

        this.current = 0;
        return BehaviorStatus.Failure;
    }
}

public class ConditionNode : BehaviorNode
{
    private readonly Func<Blackboard, bool> condition;

    public ConditionNode(Func<Blackboard, bool> condition)
    {
        this.condition = condition;
    }

    public override BehaviorStatus Tick(Blackboard blackboard)
    {
        return this.condition(blackboard) ? BehaviorStatus.Success : BehaviorStatus.Failure;
    }
}

public class ActionNode : BehaviorNode
{
    private readonly Func<Blackboard, BehaviorStatus> action;

    public ActionNode(Func<Blackboard, BehaviorStatus> action)
    {
        this.action = action;
    }

    public override BehaviorStatus Tick(Blackboard blackboard)
    {
        return this.action(blackboard);
    }
}

public static class ActionRegistry
{
    private static readonly Dictionary<string, Func<Blackboard, BehaviorStatus>> _registry = new();

    public static void Register(string name, Func<Blackboard, BehaviorStatus> func)
    {
        _registry[name] = func;
    }

    public static Func<Blackboard, BehaviorStatus> Get(string name)
    {
        return _registry.TryGetValue(name, out Func<Blackboard, BehaviorStatus> f) ? f : bb => BehaviorStatus.Failure;
    }
}

public static class TestClass
{
    private static void Test()
    {
        // 注册动作
        ActionRegistry.Register("瞄准", bb =>
        {
            Console.WriteLine("🧿 正在瞄准目标...");
            return BehaviorStatus.Success;
        });

        ActionRegistry.Register("攻击", bb =>
        {
            Console.WriteLine("💥 发起攻击！");
            return BehaviorStatus.Success;
        });

        ActionRegistry.Register("巡逻", bb =>
        {
            Console.WriteLine("🚶‍♂️ 巡逻中...");
            return BehaviorStatus.Success;
        });

        // 黑板设置
        Blackboard blackboard = new Blackboard();
        blackboard.Set("hasEnemy", true); // 可切换为 false 看效果

        // 构建行为树
        SelectorNode root = new SelectorNode(
            new SequenceNode(
                new ConditionNode(bb => bb.Get<bool>("hasEnemy")),
                new ActionNode(ActionRegistry.Get("瞄准")),
                new ActionNode(ActionRegistry.Get("攻击"))
            ),
            new ActionNode(ActionRegistry.Get("巡逻"))
        );

        // 执行行为树
        Console.WriteLine("=== Tick 行为树 ===");
        BehaviorStatus result = root.Tick(blackboard);
        Console.WriteLine($"结果: {result}");
    }
}

