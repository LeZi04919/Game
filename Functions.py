from itertools import count
import math
import random

import Game
import GameClass
import Resources

PlayerLevel:int = Game.Player.Level


def NextStep(bonus:float) -> float: #下一步
    randomStep:int = random.randint(1,6)
    levelBonus:float = math.pow(PlayerLevel,0.2)
    return randomStep * bonus * levelBonus

def GetNextArea() -> str: #随机选择一个区域
    Areas = list[str](Resources.MapAreas.keys())
    Areaseight = list[float](Resources.MapAreas.values())
    while True:
        randomArea = random.choices(Areas,Areaseight,k=1)
        if Game.Area not in randomArea  and len(randomArea) != 0:
            SetAreaStep() 
            return randomArea[0]
        
def SetAreaStep() -> None: #设置完成下一区域所需步数
    AreaStep = Game.AreaStep
    randomStep = random.uniform(1,75) * math.pow(PlayerLevel,0.2)
    AreaStep *= math.pow(PlayerLevel,0.3) + randomStep
    Game.AreaStep = int(AreaStep)

def GetEvent(Area:str) -> GameClass.Event: #随机抽取一个事件
    if Area != "新手村":
        Events = list[GameClass.Event](Resources.MapEvents[Area].keys())
        EventsWeight = list[float](Resources.MapEvents[Area].values())
        randomEvent:list[GameClass.Event] = random.choices(Events,EventsWeight,k = 1)
        return randomEvent[0]
    return GameClass.Event("Nothing","null")

def GetHealthStr(Prefab:GameClass.Prefab) -> str: #返回血条
    TotalCount = 10
    emptyCount = 10 - math.ceil((Prefab.Health / Prefab.MaxHealth * 10))
    healthStr = ""
    emptyStr = "\\"
    while True:
        if TotalCount <= emptyCount:
            healthStr += emptyStr
        else:
            healthStr += "O"
        TotalCount -= 1
        if TotalCount == 0 :
            break
    return healthStr

def Destiny() -> str: #随机选择: 无事发生0.4，事件0.3，遭遇怪物0.3
    Lucky:int = int(Game.Player.Health / Game.Player.MaxHealth * 100)
    Possibility:tuple = ("Nothing","Event","Trap")
    Traps:tuple = ("Monster","Ringleader")
    nothingPossibility = 0.4 * min(1,Lucky + 0.3)
    otherPossibility = (1 - nothingPossibility) / 2
    _Choice = random.choices(Possibility,[nothingPossibility,otherPossibility,otherPossibility],k=1)[0]
    if _Choice in "Trap":
        return random.choices(Traps,[0.8,0.2],k=1)[0]
    else:
        return _Choice
    
def CreateMonters(Count:int = 0) -> list[GameClass.Monster]: #随机生成Monster
    if Count == 0:
        monterCount = random.choices([1,2,3],k=1)[0]
    else:
        monterCount = Count
    Monsters = list[GameClass.Monster](random.choices(Resources.Monsters,k = monterCount))
    for Monster in Monsters:
        Monster.Upgrade(random.randint(max(1,PlayerLevel - 3),PlayerLevel + 3))
    return Monsters

def CreateRingleader() -> list[GameClass.Monster]: #随机生成Boss
    Ringleader:GameClass.Monster = random.choices(Resources.Ringleaders,k=1)[0]
    Ringleader.Upgrade(random.randint(max(1,PlayerLevel - 1),PlayerLevel + 1))
    Monsters = CreateMonters(2)
    Monsters.append(Ringleader)
    return Monsters
    
    
    
    


