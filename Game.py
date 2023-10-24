import GameClass

game_over = False
Area:str
Player:GameClass.Player = GameClass.Player()
TotalStepCount:int
AreaStep:int

if __name__ == "__init__":
    Area = "新手村"
    TotalStepCount = 0
    AreaStep = 100
    print("")