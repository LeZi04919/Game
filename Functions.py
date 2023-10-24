import math
import random

import Game
import GameClass
import Resources

PlayerLevel:int = Game.Player.Level


def NextStep(bonus:float):
    randomStep:int = random.randint(1,6)
    levelBonus:float = math.pow(PlayerLevel,0.2)
    return randomStep * bonus * levelBonus

def GetNextArea() -> str:
    Areas:list[str] = list[str](Resources.MapAreas.keys())
    randomArea:list[str] = []
    while True:
        randomArea = random.choices(Areas,weights=[Resources.MapAreas[Areas[0]], Resources.MapAreas[Areas[1]], Resources.MapAreas[Areas[2]],Resources.MapAreas[Areas[3]], Resources.MapAreas[Areas[4]], Resources.MapAreas[Areas[5]]],k=1)
        if Game.Area not in randomArea  and len(randomArea) != 0:
            return randomArea[0]



