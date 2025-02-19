# Unity-SimpleFSM
    最近在重写有限状态机的实现，将基于scriptable object实现状态机和状态的资产化等新功能。故将以前的写的老版本上传留档

一个Unity里的有限状态机的简易实现，提供了一个专门的`StateMachineHandler`类用于解析并执行`StateMachine`中自定义添加的继承自`IState`接口的状态类实例。

项目内提供了一个简单的示例，演示了如何创建状态和状态机、向状态机添加状态并设置过渡条件等。