class Item:
    Name = ""
    Stackable = True
    Count = 0
    MaxStackCount = 0
        
    def __init__(self,Name,Stackable,Count,MaxStackCount):
        self.Name = Name
        self.Stackable = Stackable
        self.Count = Count
        self.MaxStackCount = MaxStackCount
        
    def Use(self,UsageCount) -> bool:
        if UsageCount > self.Count:
            raise Exception(f"使用数量\"{UsageCount}\"超出了物品\"{self.Name}\"的最大数量，该物品所持数量为:{self.Count}")
        else:
            self.Count -= UsageCount
            if self.Count == 0:
                return False
            else:
                return True
    
    def Stack(self,OtherItem):
        if OtherItem.Stackable and self.Stackable:
            raise Exception(f"物品\"{self.Name}\"与物品\"{OtherItem.Name}\"中有一个或多个不可堆叠")
        if OtherItem.Name == self.Name:
            raise Exception(f"物品\"{self.Name}\"与物品\"{OtherItem.Name}\"不属于同一类")
        canStackCount = self.MaxStackCount - self.Count
        if OtherItem.Count < canStackCount:
            self.Count += OtherItem.Count
            return True
        else:
            self.Count += canStackCount
            OtherItem.Count -= canStackCount
            return OtherItem
            
class Weapon(Item):
    
    Attack = 0
    Durability = 20 #耐久度，单位为轮    
    MaxDurability = 20
    
    def __init__(self,Name,Attack,Durability):
        self.Name = Name
        self.Stackable = False
        self.Attack = Attack
        self.Durability = Durability
        
    def Attacked(self) -> bool:
        self.Durability -= 1
        if self.Durability <= 0:
            return False
        else:
            return True
        
        
class SpecialItem(Item):
    
    Effect = ""
    EffectiveRounds = 0 #生效轮数
    EffectiveObject = []
    Value = 0
    
    def __init__(self, Name, Count, MaxStackCount,Effect,EffectiveRounds,EffectiveObject,Value):
        super().__init__(Name, True, Count, MaxStackCount)
        self.Effect = Effect
        self.EffectiveRounds = EffectiveRounds
        self.EffectiveObject = EffectiveObject
        self.Value = Value
        
class Skill:
    Name = ""
    Effect = ""
    EffectiveRounds = 0 #生效轮数
    EffectiveObject = []
    Value = 0
    CoolDown = 0 #单位为轮
    
    def __init__(self,Name:str,Effect:str,EffectiveRounds:int,EffectiveObject:list[str],Value:float,CoolDown:int):
        self.Name = Name
        self.Effect = Effect
        self.EffectiveRounds = EffectiveRounds
        self.EffectiveObject = EffectiveObject
        self.Value = Value
        self.CoolDown = CoolDown
        
class Monster:
    Name = ""  #怪物名称
    Health = 0 #生命值
    Attack = 0 #攻击力
    Armor = 0 #防御力
    Dodge = 0 #闪避率
    Skills = []
    isBoss = False

    def __init__(self,Name:str,Health:float,Armor:float,Attack:float,Dodge:float,isBoss:bool,Skills:list[Skill]):
        self.Name = Name
        self.Health = Health
        self.Armor = Armor
        self.Attack = Attack
        self.Dodge = Dodge
        self.isBoss = isBoss
        self.Skills = Skills
        
class Player:
    Health = 20
    Attack = 5
    Armor = 1
    Experience = 0
    Level = 1
    Buff = {} # Buff/DeBuff名 : Buff层数
    Items = []
    Skills = [ 
        Skill
        (
            "强力击",
            "AttackUp",
            1,
            ["Player"],
            2.0,
            3
        )
    ]

    def __init__(self):
        pass