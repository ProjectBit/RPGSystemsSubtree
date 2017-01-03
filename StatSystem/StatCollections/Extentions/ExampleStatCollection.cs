﻿using UnityEngine;

namespace Systems.StatSystem
{
    /// <summary>
    /// Example Stat Collection
    /// In reality we would make stat collections like-
    /// WarriorStatCollection
    ///     then in the configure stats part - define what kind of base stats and linked attributes
    ///     a warrior would have
    /// </summary>
    public class ExampleStatCollection : StatCollection
    {
        protected override void ConfigureStats()
        {
            #region ATTRIBUTES
            var stamina = CreateOrGetStat<StatAttribute>(StatType.Stamina);
            stamina.StatName = "Stamina";
            stamina.StatBaseValue = 0;

            var stength = CreateOrGetStat<StatAttribute>(StatType.Strength);
            stength.StatName = "Strength";
            stength.StatBaseValue = 5;

            var endurance = CreateOrGetStat<StatAttribute>(StatType.Endurance);
            endurance.StatName = "Endurance";
            endurance.StatBaseValue = 6;

            var agility = CreateOrGetStat<StatAttribute>(StatType.Agility);
            agility.StatName = "Agility";
            agility.StatBaseValue = 6;

            var intellegence = CreateOrGetStat<StatAttribute>(StatType.Intellegence);
            intellegence.StatName = "Intellegence";
            intellegence.StatBaseValue = 0;
            #endregion

            #region STATS
            /*
            The amount of health the entity has available
            */
            var health = CreateOrGetStat<StatRegeneratable>(StatType.Health);
            health.StatName = "Health";
            health.StatBaseValue = 100;
            health.BaseSecondsPerPoint = 100;
            //Stamina = 10; and our ratio is 10; 10*10 = 0; health = 100; health + Stamina = 200
            health.AddLinker(new ExampleStatLinker(CreateOrGetStat<StatAttribute>(StatType.Stamina), 10f)); // health should be 200
                                                                                                          //Endurance = 8; and our ratio is 1; 1*8= 8; health.SecondsPerPoint = 1; health.SecondsPerPoint - Endurance = 8
            health.AddLinker(new ExampleStatLinker(CreateOrGetStat<StatAttribute>(StatType.Endurance), 5f, true));
            health.UpdateLinkers();
            health.RestoreCurrentValueToMax();

            /*
            The amount of magic the entity can use
            */
            var magic = CreateOrGetStat<StatRegeneratable>(StatType.Magic);
            magic.StatName = "Magic";
            magic.StatBaseValue = 10;
            magic.BaseSecondsPerPoint = 10;
            magic.RestoreCurrentValueToMax();

            /*
            The amount of mana the entity can use
            */
            var mana = CreateOrGetStat<StatRegeneratable>(StatType.Mana);
            mana.StatName = "Mana";
            mana.StatBaseValue = 100;
            mana.BaseSecondsPerPoint = 10;
            mana.AddLinker(new ExampleStatLinker(CreateOrGetStat<StatAttribute>(StatType.Intellegence), 50f));
            mana.UpdateLinkers();
            mana.RestoreCurrentValueToMax();

            /*
            How much armor the entity has
            */
            var armor = CreateOrGetStat<StatVital>(StatType.Armor);
            armor.StatName = "Armor";
            armor.StatBaseValue = 50;
            armor.RestoreCurrentValueToMax();

            /*
            How much protection the armor gives the entity - percentage of the hit
            */
            var armorProtection = CreateOrGetStat<StatAttribute>(StatType.ArmorProtection);
            armorProtection.StatName = "ArmorProtection";
            armorProtection.StatBaseValue = 50;

            /*
            How quickly the entity moves in the scene
            */
            var speed = CreateOrGetStat<StatVital>(StatType.Speed);
            speed.StatName = "Speed";
            speed.StatBaseValue = 1;
            speed.AddLinker(new ExampleStatLinker(CreateOrGetStat<StatAttribute>(StatType.Agility), 0.01f));
            speed.RestoreCurrentValueToMax();

            /*
            The amount of Items the entity can carry with
            */
            var inventory = CreateOrGetStat<StatVital>(StatType.InventoryCap);
            inventory.StatName = "Inventory";
            inventory.StatBaseValue = 5;
            inventory.AddLinker(new ExampleStatLinker(CreateOrGetStat<StatAttribute>(StatType.Strength), 1f));
            inventory.UpdateLinkers();
            #endregion
        }

        void FixedUpdate()
        {
            RegenStats();
        }

        //Only way to make regen stats work
        void RegenStats()
        {
            foreach (var i in GetAllRegeneratingStats)
            {
                if (i.StatCurrentValue != i.StatValue)
                {
                    if (Time.time > i.TimeForNextRegen)
                    {
                        i.Regenerate();
                        i.TimeForNextRegen = Time.time + i.SecondsPerPoint;
                    }
                }
            }
        }
    }
}