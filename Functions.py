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
    Areas:list[str] = Resources.MapAreas.keys()
    randomArea:str = ""
    while True:
        randomArea = random.choices(Areas, weights=[Resources[Areas[0]], Resources[Areas[1]], Resources[Areas[2]],Resources[Areas[3]], Resources[Areas[4]], Resources[Areas[5]]],k=1)
        if randomArea != Game.Area and randomArea != "":
            return randomArea



