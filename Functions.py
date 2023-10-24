import math
import random

import Game
import GameClass
import Resources

PlayerLevel:int = Game.Player.Level


def NextStep(bonus:float) -> float:
    randomStep:int = random.randint(1,6)
    levelBonus:float = math.pow(PlayerLevel,0.2)
    return randomStep * bonus * levelBonus

def GetNextArea() -> str:
    Areas = list[str](Resources.MapAreas.keys())
    Areaseight = list[float](Resources.MapAreas.values())
    while True:
        randomArea = random.choices(Areas,Areaseight,k=1)
        if Game.Area not in randomArea  and len(randomArea) != 0:
            SetAreaStep() #设置完成下一区域所需步数
            return randomArea[0]
        
def SetAreaStep() -> None:
    AreaStep = Game.AreaStep
    randomStep = random.uniform(1,75) * math.pow(PlayerLevel,0.2)
    AreaStep *= math.pow(PlayerLevel,0.3) + randomStep
    Game.AreaStep = int(AreaStep)



def GetEvent(Area:str) -> GameClass.Event:
    if Game.Area != "新手村":
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
    
    
    
    


