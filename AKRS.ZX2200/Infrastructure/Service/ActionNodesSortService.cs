namespace AKRS.ZX2200.Infrastructure.Service
{
    using System.Collections.Generic;

    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Services;
    using AKRS.ZX2200.TransportUnitSystem;

    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 动作排序的帮助类
    /// </summary>
    public static class ActionNodesSortService
    {
        /// <summary>
        /// 动作排序
        /// </summary>
        /// <param name="actionNodes">动作名称集合</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <param name="processStepNames">名称</param>
        /// <param name="substrate">基板号</param>
        /// <returns>结果</returns>
        public static List<ActionNodeMessage> SortBondPositionActionNode(
            List<ActionNode> actionNodes,
            int start,
            int end,
            List<string> processStepNames,
            int substrate)
        {
            // 需要返回的对象
            List<ActionNodeMessage> actionNodeMessages = new List<ActionNodeMessage>();

            // 判断列的长度，如果过长证明列的数量不正确
            if (actionNodes.Count <= 0 || processStepNames == null || processStepNames.Count <= 0)
            {
                return actionNodeMessages;
            }

            // 装载所有基岛的集合
            List<int> islandCount = new List<int>();

            for (int i = start; i <= end; i++)
            {
                islandCount.Add(i);
            }

            // 遍历所有的动作
            for (int i = 0; i < actionNodes.Count; i++)
            {
                if (actionNodes[i].ActionLevel == ActionLevelEnum.BondPosition)
                {
                    // 获取动作的名称，后续和动作一一对应
                    string actionName = actionNodes[i].Name;

                    if (actionNodes[i].ActionSortType == ActionTypeEnum.TransportUnit || actionNodeMessages.Count == 0)
                    {
                        for (int j = 0; j < islandCount.Count; j++)
                        {
                            for (int k = 0; k < processStepNames.Count; k++)
                            {
                                actionNodeMessages.Add(new ActionNodeMessage(actionName, substrate, islandCount[j], processStepNames[k]));
                            }
                        }
                    }
                    else if (actionNodes[i].ActionSortType == ActionTypeEnum.Island)
                    {
                        // 插入方式为基岛插入
                        int beforeNameNumber = 0;
                        string beforeName = actionNodes[actionNodes.IndexOf(actionNodes[i]) - 1].Name;
                        for (int j = 0; j < actionNodeMessages.Count; j++)
                        {
                            if (actionNodeMessages[i].ActionNodeName == beforeName)
                            {
                                beforeNameNumber++;
                                if (beforeNameNumber >= processStepNames.Count)
                                {
                                    // 一个基岛里面有多少个焊点
                                    for (int k = 0; k < processStepNames.Count; k++)
                                    {
                                        actionNodeMessages.Insert(
                                            j + 1,
                                            new ActionNodeMessage(
                                                actionNodes[i].Name,
                                                substrate,
                                                actionNodeMessages[i].Module,
                                                actionNodeMessages[i].ProcessStepName));
                                        i++;
                                    }

                                    beforeNameNumber = 0;
                                }
                            }
                        }
                    }
                    else if (actionNodes[i].ActionSortType == ActionTypeEnum.BondPosition)
                    {
                        // 插入方式为挨个插入
                        string beforeName = actionNodes[actionNodes.IndexOf(actionNodes[i]) - 1].Name;
                        for (int j = 0; j < actionNodeMessages.Count; j++)
                        {
                            if (actionNodeMessages[j].ActionNodeName == beforeName/*&& actionName != "S2BondPositionMeasureHeightActionNode"*/)
                            {
                                actionNodeMessages.Insert(
                                    j + 1,
                                    new ActionNodeMessage(
                                        actionNodes[i].Name,
                                        substrate,
                                        actionNodeMessages[j].Module,
                                        actionNodeMessages[j].ProcessStepName));
                                j++;
                            }
                        }
                    }
                }
            }

            return actionNodeMessages;
        }

        /// <summary>
        /// 对其他动作进行排序
        /// </summary>
        /// <param name="actionNodes">动作名称</param>
        /// <param name="actionNodeMessages">集合</param>
        /// <param name="productConfiguration">TU的配置文件</param>
        /// <returns>结果</returns>
        public static List<ActionNodeMessage> SortOtherActionNode(
            List<ActionNode> actionNodes,
            List<ActionNodeMessage> actionNodeMessages,
            ProductConfiguration productConfiguration)
        {
            if (actionNodes == null || actionNodeMessages == null)
            {
                return actionNodeMessages;
            }

            if (actionNodes.Count == 0 || actionNodeMessages.Count == 0)
            {
                return actionNodeMessages;
            }

            // 找到基岛拍照 , 这个目前只有这一个动作，如果有其他的动作，后续再增加
            List<ActionNode> actions = actionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Module);

            // 顺序反转，定位放在测高前面
            actions.Reverse();

            foreach (ActionNode action in actions)
            {
                if (action.ActionSortType == ActionTypeEnum.TransportUnit || action.ActionSortType == ActionTypeEnum.Substrate)
                {
                    // 遍历所有substrate
                    for (int i = 0; i < productConfiguration.SubstrateConfig.Count; i++)
                    {
                        // 最前面注入，及基岛全部拍完
                        for (int j = 0; j < productConfiguration.ModuleConfig.Count; j++)
                        {
                            actionNodeMessages.Insert(
                                j + i * productConfiguration.ModuleConfig.Count,
                                new ActionNodeMessage(action.Name, i+1, j + 1, null));
                        }
                    }
                }
                else
                {
                    int[,] moduleActionIndex = new int[productConfiguration.SubstrateConfig.Count,
                        productConfiguration.ModuleConfig.Count];

                    for (int i = 0; i < moduleActionIndex.GetLength(0); i++)
                    {
                        for (int j = 0; j < moduleActionIndex.GetLength(1); j++)
                        {
                            moduleActionIndex[i, j] = 1;
                        }
                    }

                    // 找到开始一个module的最开始的动作
                    for (int j = 0; j < actionNodeMessages.Count; j++)
                    {
                        if (moduleActionIndex[actionNodeMessages[j].Substrate - 1, actionNodeMessages[j].Module - 1] == 1)
                        {
                            moduleActionIndex[actionNodeMessages[j].Substrate - 1, actionNodeMessages[j].Module - 1] = 0;
                            actionNodeMessages.Insert(
                                j,
                                new ActionNodeMessage(
                                    action.Name,
                                    actionNodeMessages[j].Substrate,
                                    actionNodeMessages[j].Module,
                                    null));
                        }
                    }
                }
            }

            // 找到基板拍照 目前只有这一个，先这样写
            List<ActionNode> subActionNodes = actionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Substrate);

            // 顺序反转，定位放在测高前面
            subActionNodes.Reverse();

            foreach (ActionNode subActionNode in subActionNodes)
            {
                if (subActionNode.ActionSortType == ActionTypeEnum.TransportUnit
                    || subActionNode.ActionSortType == ActionTypeEnum.Substrate)
                {
                    // 最前面注入，及基岛全部拍完
                    for (int i = 0; i < productConfiguration.SubstrateConfig.Count; i++)
                    {
                        actionNodeMessages.Insert(
                            i,
                            new ActionNodeMessage(subActionNode.Name, i + 1, 0, null));
                    }

                    return actionNodeMessages;
                }
                else
                {
                    int[] subActionIndex = new int[productConfiguration.SubstrateConfig.Count];

                    for (int i = 0; i < subActionIndex.GetLength(0); i++)
                    {
                        subActionIndex[i] = 1;
                    }

                    // 找到开始一个module的最开始的动作
                    for (int j = 0; j < actionNodeMessages.Count; j++)
                    {
                        if (subActionIndex[actionNodeMessages[j].Substrate - 1] == 1)
                        {
                            subActionIndex[actionNodeMessages[j].Substrate - 1] = 0;
                            actionNodeMessages.Insert(
                                j,
                                new ActionNodeMessage(
                                    subActionNode.Name,
                                    actionNodeMessages[j].Substrate,
                                    0,
                                    null));
                        }
                    }
                }
            }

            // 找到TU拍照
            List<ActionNode> tuActionNodes = actionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Carrier);

            // 顺序反转，定位放在测高前面
            tuActionNodes.Reverse();

            foreach (ActionNode tuActionNode in tuActionNodes)
            {
                actionNodeMessages.Insert(
                    0,
                    new ActionNodeMessage(tuActionNode.Name, 0, 0, null));
            }

            ActionNode otherActionNode = actionNodes.Find(it => it.ActionLevel == ActionLevelEnum.Other);

            if (otherActionNode != null)
            {
                int divideSub = (int)productConfiguration.SubstrateConfig.Count / 2;

                for (int i = 0; i < actionNodeMessages.Count; i++)
                {
                    if (actionNodeMessages[i].Substrate == 1)
                    {
                        actionNodeMessages.Insert(
                            i,
                            new ActionNodeMessage(otherActionNode.Name, 0, 0, null));
                        break;
                    }
                }
            }

            return actionNodeMessages;
        }

        /// <summary>
        /// 对其他动作进行排序
        /// </summary>
        /// <param name="actionNodes">动作名称</param>
        /// <param name="actionNodeMessages">集合</param>
        /// <param name="transportUnit">载具</param>
        /// <returns>结果</returns>
        public static List<ActionNodeMessage> SortS2OtherActionNode(
            List<ActionNode> actionNodes,
            List<ActionNodeMessage> actionNodeMessages,
            TransportUnit transportUnit)
        {
            if (actionNodes == null || actionNodeMessages == null)
            {
                return actionNodeMessages;
            }

            if (actionNodes.Count == 0 || actionNodeMessages.Count == 0)
            {
                return actionNodeMessages;
            }

            // 找到基岛拍照 , 这个目前只有这一个动作，如果有其他的动作，后续再增加
            List<ActionNode> actions = actionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Module);

            // 顺序反转，定位放在测高前面
            actions.Reverse();

            foreach (ActionNode action in actions)
            {
                if (action.ActionSortType == ActionTypeEnum.TransportUnit || action.ActionSortType == ActionTypeEnum.Substrate|| action.ActionSortType == ActionTypeEnum.Island)
                {
                    // 遍历所有substrate
                    for (int i = 0; i < transportUnit.GetSubstrateCount(); i++)
                    {
                        // 最前面注入，及基岛全部拍完
                        for (int j = 0; j < transportUnit.GetModuleCount(); j++)
                        {
                            actionNodeMessages.Insert(
                                j + i * transportUnit.GetModuleCount(),
                                new ActionNodeMessage(
                                    action.Name,
                                    i + 1,
                                    j + 1,
                                    actionNodeMessages[0].ProcessStepName));
                        }
                    }
                }
                else
                {
                    // 索引
                    int moduleIndex = 1;

                    int substrateIndex = 1;

                    // 找到开始一个module的最开始的动作
                    for (int j = 0; j < actionNodeMessages.Count; j++)
                    {
                        //if (actionNodeMessages[j].Module == moduleIndex && actionNodeMessages[j].Substrate == substrateIndex && actionNodeMessages[j].ActionNodeName == "BondActionNode")
                        if (actionNodeMessages[j].Module == moduleIndex && actionNodeMessages[j].Substrate == substrateIndex)
                        {
                            actionNodeMessages.Insert(
                                j,
                                new ActionNodeMessage(
                                    action.Name,
                                    substrateIndex,
                                    moduleIndex,
                                    actionNodeMessages[0].ProcessStepName));
                            moduleIndex++;
                            j++;

                            if (moduleIndex == transportUnit.GetModuleCount() + 1)
                            {
                                moduleIndex = 1;
                                substrateIndex++;
                            }

                            if (substrateIndex == transportUnit.GetSubstrateCount() + 1)
                            {
                                break;
                            }
                        }

                    }
                }
            }

            // 找到基板拍照 目前只有这一个，先这样写
            List<ActionNode> subActionNodes = actionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Substrate);

            // 顺序反转，定位放在测高前面
            subActionNodes.Reverse();

            foreach (ActionNode subActionNode in subActionNodes)
            {
                if (subActionNode.ActionSortType == ActionTypeEnum.TransportUnit
                    || subActionNode.ActionSortType == ActionTypeEnum.Substrate)
                {
                    // 最前面注入，及基岛全部拍完
                    for (int i = 0; i <= transportUnit.GetSubstrateCount(); i++)
                    {
                        actionNodeMessages.Insert(
                            i,
                            new ActionNodeMessage(subActionNode.Name, i, 0, actionNodeMessages[0].ProcessStepName));
                    }

                    return actionNodeMessages;
                }
                else
                {
                    int substrateIndex = 1;

                    // 找到开始一个module的最开始的动作
                    for (int j = 0; j < actionNodeMessages.Count; j++)
                    {
                        if (actionNodeMessages[j].Substrate == substrateIndex)
                        {
                            actionNodeMessages.Insert(
                                j,
                                new ActionNodeMessage(
                                    subActionNode.Name,
                                    substrateIndex,
                                    0,
                                    actionNodeMessages[0].ProcessStepName));
                            substrateIndex++;
                            j++;
                        }
                    }
                }
            }

            // 找到TU拍照
            List<ActionNode> tuActionNodes = actionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Carrier);

            // 顺序反转，定位放在测高前面
            tuActionNodes.Reverse();

            foreach (ActionNode tuActionNode in tuActionNodes)
            {
                actionNodeMessages.Insert(
                    0,
                    new ActionNodeMessage(tuActionNode.Name, 0, 0, actionNodeMessages[0].ProcessStepName));
            }

            return actionNodeMessages;
        }
    }
}
