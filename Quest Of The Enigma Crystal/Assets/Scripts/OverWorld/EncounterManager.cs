using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class EncounterManager : MonoBehaviour
{
    List<NormalEncounter> normalEncounters;
    List<GoldEncounter> goldEncounters;
    List<HealEncounter> healEncounters;
    List<TakeDamageEncounter> takeDamageEncounters;
    List<EnemyEncounter> enemyEncounters;
    List<RandomEnemyEncounter> randomEnemyEncounters;
    EnemyEncounter bossEncounter;
    //public Dictionary<EncounterType, List<Encounter>> encounterDictionary = new Dictionary<EncounterType, List<Encounter>>();
    void Awake()
    {
        bossEncounter = new EnemyEncounter(
            "warlock_sprite",
            1000,
            new List<GameObject>
            {
                Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                Instantiate(WizardDataManager.instance.Warlock, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
            },
            "You enter the castle through the main gate. In the castle courtyard, ghastly revenants are awaiting you. Among them a taller hooded figure stands with a big staff and a book. Its the Warlock you have heard rumors about. The doors slams shut behind you. There is no turning back... "); //You encounter a group of revenants, that are guarding the entrance to the castle. They are blocking your path, and you have no choice but to fight them.

        normalEncounters = new List<NormalEncounter>
        {
            new NormalEncounter(
                "frog_sprite",
                "In a small pond, your party encounters a gathering of frogs, each one croaking in a rhythmic pattern. You watch as they hop around and interact with each other, creating a lively scene in the tranquil setting, before moving on."),
            new NormalEncounter(
                "mainchar_sprite",
                "You try to crack a joke to lighten the mood in this very dark and scary forest, however no one laughs. Your party continues in awkward silence..."),
            new NormalEncounter(
                "mainchar_sprite",
                "A mischievous squirrel steals a shiny trinket from your party's belongings. After a brief chase, the squirrel drops it, and your party is left chuckling at the woodland thief's antics."),
            new NormalEncounter(
                "mainchar_sprite",
                "Amidst the ancient trees, your party encounters a peculiar stone with an intricate carving. It's a riddle, but solving it leads to nothing more than a sense of accomplishment as the forest remains unchanged."),

        };
        //encounterDictionary.Add(EncounterType.NORMAL, NormalEncounters);

        goldEncounters = new List<GoldEncounter>
        {
            new GoldEncounter(
                "goldstack_sprite",
                "- 15 gold is added to your inventory.",
                15,
                "You find a small bag of gold on the ground. You pick it up and put it in your bag."),
            new GoldEncounter(
                "goldstack_sprite",
                "- 15 gold is added to your inventory.",
                15,
                "You find a small bag of gold on the ground. You pick it up and put it in your bag."),
            new GoldEncounter(
                "goldstack_sprite",
                "- 30 gold is added to your inventory.",
                30,
                "You find a dead corpse on the ground. You find gold and put it in your bag."),
            new GoldEncounter(
                "goldstack_sprite",
                "- 15 gold is added to your inventory.",
                100,
                "You find a hidden chest of gold. You pick it up and put it in your bag."),
        };
        //encounterDictionary.Add(EncounterType.GOLD, GoldEncounters);

        healEncounters = new List<HealEncounter>
        {
            new HealEncounter(
                "mainchar_sprite",
                "- A unit in your roster has been healed by 10 points",
                10,
                "You stumble upon a small clearing in the woods, with sunlight peeking through the trees and a soft breeze. Here, you relax, eat, and feel refreshed, surrounded by nature and the smell of wildflowers."),
            new HealEncounter(
                "mainchar_sprite",
                "- A unit in your roster has been healed by 5 points",
                5,
                "Walking down the road, one of your companions picks a tasty red apple from a tree.")
        };
        //encounterDictionary.Add(EncounterType.HEAL, HealEncounters);

        takeDamageEncounters = new List<TakeDamageEncounter>
        {
            new TakeDamageEncounter(
                "mainchar_sprite",
                "- A unit in your roster has taken 4 points of damage",
                5,
                "Your party takes a break at a small stream. One of your companions takes off their shoes and dips their feet in the water. Suddenly, your companion lets out a yelp. Looking at their feet, you see a leech attached to your companions foot, sucking their blood. Your companion quickly pull it off, but the wound is already bleeding."),
            new TakeDamageEncounter(
                "mainchar_sprite",
                "- A unit in your roster has taken 5 points of damage",
                5,
                "A misjudged leap across a moss-covered chasm results in a companion spraining their ankle on the landing. Though visibly in pain, they grit their teeth, and the party keeps moving, determined to put distance between them and potential threats.")
        };
        //encounterDictionary.Add(EncounterType.TAKEDMG, TakeDamageEncounters);

        enemyEncounters = new List<EnemyEncounter>
        {
            new EnemyEncounter(
                "revenant_sprite",
                50,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "Vengeful spirits emerge, ethereal forms twisted by rage. The air chills as your party readies for a confrontation with these hostile revenants."),
            new EnemyEncounter(
                "wolf_sprite",
                40,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Wolf, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Wolf, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Wolf, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Wolf, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "Out from the dense forest, a group of wolfs are charging against you."
                ),
            new EnemyEncounter(
                "infantryman_sprite",
                100,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Infantryman, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Infantryman, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Archer, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "You run into two soldiers, that are patrolling the area. You see one experienced fighters with an archers behind him.."
                ),
            new EnemyEncounter(
                "orc_sprite",
                75,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Orc, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Orc, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Orc, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "A group of orcs ambushes your group."
                ),
            new EnemyEncounter(
                "revenant_sprite",
                75,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Revenant, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "Spooky ghosts"),
            new EnemyEncounter(
                "paladin_sprite",
                100,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Paladin, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "A very intimidating paladin is blocking your path. Hes out to kill you all"
                ),
            new EnemyEncounter(
                "champion_sprite",
                100,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Champion, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "A very intimidating champion is blocking your path. Hes out to kill you all"
                )
        };
        //encounterDictionary.Add(EncounterType.ENEMY, EnemyEncounters);

        randomEnemyEncounters = new List<RandomEnemyEncounter>
        {
            new RandomEnemyEncounter(
                "infantryman_sprite",
                "- You can either fight them or give them gold (100).",
                50,
                150,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Infantryman, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Infantryman, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Archer, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Archer, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "You run into a group of soldiers, that are patrolling the area. You see two experienced fighters with two archers behind them. They demand gold as taxes."
                ),
            new RandomEnemyEncounter(
                "pikeman_sprite",
                "- You can either fight them or give them gold (10).",
                10,
                30,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Pikeman, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Pikeman, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "You run into a group of soldiers, that are patrolling the area. You see two weak fighters. They demand gold as taxes."
                ),
            new RandomEnemyEncounter(
                "orc_sprite",
                "- You can either fight them or give them gold (25).",
                25,
                75,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Orc, WizardDataManager.instance.EnemyUnitsParentGameObject.transform),
                    Instantiate(WizardDataManager.instance.Orc, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "A group of Orcs ambushes your group. You see two orcs. They demand gold"
                ),
            new RandomEnemyEncounter(
                "paladin_sprite",
                "- You can either fight them or give them gold (50).",
                50,
                100,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Paladin, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "A very intimidating paladin is blocking your path. He demands gold."
                ),
            new RandomEnemyEncounter(
                "champion_sprite",
                "- You can either fight them or give them gold (50).",
                50,
                100,
                new List<GameObject>
                {
                    Instantiate(WizardDataManager.instance.Champion, WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                },
                "A very intimidating champion is blocking your path. He demands gold."
                )

        };
        //encounterDictionary.Add(EncounterType.RANDOM_ENEMY, RandomEnemyEncounters);
    }

    public IEncounter GetRandomEncounter()
    {
        var randomNumber = UnityEngine.Random.Range(0, 100);
        if (randomNumber < 20)
        {
            return randomEnemyEncounters[Random.Range(0, randomEnemyEncounters.Count)];
        }
        else if (randomNumber < 40)
        {
            return goldEncounters[Random.Range(0, goldEncounters.Count)];
        }
        else if (randomNumber < 60)
        {
            return healEncounters[Random.Range(0, healEncounters.Count)];
        }
        else if (randomNumber < 80)
        {
            return takeDamageEncounters[Random.Range(0, takeDamageEncounters.Count)];
        }
        else
        {
            return normalEncounters[Random.Range(0, normalEncounters.Count)];
        }
    }

    public EnemyEncounter GetEnemyEncounter()
    {
        return enemyEncounters[Random.Range(0, enemyEncounters.Count)];
    }

    public EnemyEncounter GetBossEncounter()
    {
        return bossEncounter;
    }

}

public interface IEncounter
{
    EncounterType encounterType { get; set; }
    string spriteName { get; set; }
    string contentText { get; set; }
}

public interface IEncounterEffect : IEncounter
{
    string effectText { get; set; }
}

public interface IEncounterEnemy : IEncounter
{
    public List<GameObject> enemyUnits { get; set; }
    public int battleReward { get; set; }
}

public class NormalEncounter : IEncounter
{
    public EncounterType encounterType { get; set; }
    public string spriteName { get; set; }
    public string contentText { get; set; }

    public NormalEncounter(string encounterSpritePath, string encounterText)
    {
        encounterType = EncounterType.NORMAL;
        this.spriteName = encounterSpritePath;
        this.contentText = encounterText;
    }
}

public class GoldEncounter : IEncounter, IEncounterEffect
{
    public EncounterType encounterType { get; set; }
    public string spriteName { get; set; }
    public string contentText { get; set; }
    public string effectText { get; set; }

    public int goldFound;
    public GoldEncounter(string encounterSpritePath, string effectText, int goldFound, string encounterText)
    {
        encounterType = EncounterType.GOLD;
        this.spriteName = encounterSpritePath;
        this.contentText = encounterText;
        this.effectText = effectText;
        this.goldFound = goldFound;
    }
}

public class HealEncounter : IEncounter, IEncounterEffect
{
    public EncounterType encounterType { get; set; }
    public string spriteName { get; set; }
    public string contentText { get; set; }
    public string effectText { get; set; }
    public int healthHealed;
    public HealEncounter(string encounterSpritePath, string effectText, int healthHealed, string encounterText)
    {
        encounterType = EncounterType.HEAL;
        this.spriteName = encounterSpritePath;
        this.contentText = encounterText;
        this.effectText = effectText;
        this.healthHealed = healthHealed;
    }
}

public class TakeDamageEncounter : IEncounter, IEncounterEffect
{
    public EncounterType encounterType { get; set; }
    public string spriteName { get; set; }
    public string contentText { get; set; }
    public string effectText { get; set; }
    public int damageTaken;
    public TakeDamageEncounter(string encounterSpritePath, string effectText, int healthLost, string encounterText)
    {
        encounterType = EncounterType.TAKEDMG;
        this.spriteName = encounterSpritePath;
        this.contentText = encounterText;
        this.damageTaken = healthLost;
        this.effectText = effectText;
    }
}

public class EnemyEncounter : IEncounter, IEncounterEnemy
{
    public EncounterType encounterType { get; set; }
    public string spriteName { get; set; }
    public string contentText { get; set; }
    public List<GameObject> enemyUnits { get; set; }
    public int battleReward { get; set; }

    public EnemyEncounter(string encounterSpritePath, int battleRewardInGold, List<GameObject> enemyUnits, string encounterText)
    {
        encounterType = EncounterType.ENEMY;
        this.spriteName = encounterSpritePath;
        this.contentText = encounterText;
        this.enemyUnits = enemyUnits;
        this.battleReward = battleRewardInGold;
    }
}

public class RandomEnemyEncounter : IEncounter, IEncounterEffect, IEncounterEnemy
{
    public EncounterType encounterType { get; set; }
    public string spriteName { get; set; }
    public string contentText { get; set; }
    public string effectText { get; set; }
    public List<GameObject> enemyUnits { get; set; }
    public int battleReward { get; set; }

    public int bribeAmount;
    public RandomEnemyEncounter(string encounterSpritePath, string effectText, int bribeAmount, int battleRewardInGold, List<GameObject> enemyUnits, string encounterText)
    {
        encounterType = EncounterType.RANDOM_ENEMY;
        this.spriteName = encounterSpritePath;
        this.contentText = encounterText;
        this.bribeAmount = bribeAmount;
        this.effectText = effectText;
        this.enemyUnits = enemyUnits;
        this.battleReward = battleRewardInGold;
    }
}

public enum EncounterType
{
    NORMAL,
    GOLD,
    TAKEDMG,
    HEAL,
    ENEMY,
    RANDOM_ENEMY
};