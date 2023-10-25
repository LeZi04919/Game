import GameClass
import sys

Items:list = [
    GameClass.Weapon(
        "双手剑",   
        3,
        20        
    ),
    GameClass.WearableArmor(
        "精良的护甲",
        2,
        0
        ),
    GameClass.WearableArmor(
        "光滑的护甲",
        1,
        0.2
    ),
    GameClass.SpecialItem(
        "HP恢复药",
        1,
        20,
        "HealthUP",
        3,
        [GameClass.Player],
        4              
    ),
    GameClass.SpecialItem(
        "技能冷却加速剂",
        1,
        10,
        "CoolDownUP",
        0,
        [GameClass.Player],
        1        
    ),
    GameClass.Weapon(
        "因果剑",
        1.5,
        15
    ),
    GameClass.Item(
        "硬币",
        True,
        1,
        sys.maxsize
    )    
]

Monsters:list[GameClass.Monster] = [
    GameClass.Monster("史莱姆",5,0,2,0.5,False,[]),
    GameClass.Monster("红史莱姆",5,0,2,0.5,False,[]),
    GameClass.Monster("蓝史莱姆",5,0,2,0.5,False,[]),
    GameClass.Monster("哥布林",15,1,5,0,False,[],1),
    GameClass.Monster("精英哥布林",20,3,6.5,0.05,False,[],1),
    GameClass.Monster("兽人",20,4,8,0,False,[],1),
    GameClass.Monster("精英兽人",30,5,12,0,False,[],1)   
]

Ringleaders:list[GameClass.Monster] = [
    GameClass.Monster("史莱姆王",50,2.5,10,0.75,True,[],1),
    GameClass.Monster("龙",100,5,15,0,True,[],1)
]

Events:list[GameClass.Event] = [
    GameClass.Event("宝箱事件","Adventure",[GameClass.Monster("宝箱怪",30,5,10,0,False,
        [
            GameClass.Skill("宝箱护盾","DodgeUp",1,["Monster"],0.9,4),
            GameClass.Skill("强力撕咬","AttackUp",1,["Player"],2.5,4)
        ],
    )]),
    GameClass.Event("前辈","Adventure"),
    GameClass.Event("阿哈玩偶","Adventure"),
    GameClass.Event("动物聚会","Adventure",[Monsters[0],Monsters[1],Monsters[2],Monsters[3],Monsters[4],Monsters[5],Monsters[6]]),
    GameClass.Event("坎诺特","Shop"),
    GameClass.Event("史莱姆群","Trap",[Monsters[0],Monsters[1],Monsters[2]]),
    GameClass.Event("灼伤","Status"),
    GameClass.Event("冻伤","Status"),
    GameClass.Event("北极熊","Trap",[GameClass.Monster("北极熊",35,5,15,0,False,
        [
            GameClass.Skill("强力击","AttackUp",1,["Self"],1.75,5),
            GameClass.Skill("震慑","ArmorDown",3,["Player"],0.75,3),
            GameClass.Skill("防护强化","ArmorUp",3,["Self"],1.5,2)
        ]
    )]),
    GameClass.Event("缺水","Status"),
    GameClass.Event("昏厥","Status"),
    GameClass.Event("普通商人","Shop"),
    GameClass.Event("卫兵打劫","Trap",[GameClass.Monster("城市卫兵",25,3,10,0,False,
        [
            GameClass.Skill("强力穿刺","AttackUp",1,["Self"],1.25,3),
            GameClass.Skill("架起护盾","ArmorUp",2,["Self"],2,3) 
         ]        
    )])

]

MapAreas:dict[str,float] = {"草原":0.3,"平原":0.3,"火山":0.1,"冰原":0.1,"沙漠":0.05,"城市":0.15} # 区域 : 进入概率

MapEvents:dict[str,dict[GameClass.Event,float]] = {
    "草原":{Events[0]:0.2,Events[1]:0.2,Events[2]:0.3,Events[3]:0.1,Events[4]:0.2},
    "平原":{Events[4]:0.2,Events[1]:0.2,Events[5]:0.4,Events[2]:0.2},
    "火山":{Events[6]:0.3,Events[5]:0.4,Events[1]:0.3},
    "冰原":{Events[7]:0.3,Events[5]:0.4,Events[1]:0.2,Events[8]:0.1},
    "沙漠":{Events[9]:0.3,Events[1]:0.5,Events[10]:0.3},
    "城市":{Events[4]:0.4,Events[11]:0.4,Events[12]:0.2}
}
