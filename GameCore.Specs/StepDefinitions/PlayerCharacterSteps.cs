using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace GameCore.Specs.StepDefinitions
{
    [Binding]
    public class PlayerCharacterSteps
    {
        private readonly PlayerCharacterStepsContext _context;

        public PlayerCharacterSteps(PlayerCharacterStepsContext context)
        {
            _context = context;
        }
        //private PlayerCharacter _player.Player;
        //[Given(@"I'm a new player")]
        //public void GivenImANewPlayer()
        //{
        //    _contex.Player = new PlayerCharacter();
        //}

        [When("I take (.*) damage")]
        public void WhenITakeDamage(int damage)
        {
            _context.Player.Hit(damage);
        }

        #region Tag
        //duplice metot scope ve tag ile elf için sadece burayı çalıştıracak şilde filtre uygular
        [When("I take (.*) damage")]
        [Scope(Tag ="elf")]
        public void WhenITakeDamageAsAnElf(int damage)
        {
            _context.Player.Hit(damage);
        }
        #endregion

        //[When(@"I take 0 damage")]
        //public void WhenITakeDamage()
        //{
        //    _contex.Player.Hit(0);
        //}

        [Then(@"My health should now be (.*)")]
        public void ThenMyHealthShouldNowBe(int expectedHealth)
        {
            Assert.Equal(expectedHealth, _context.Player.Health);
        }

        //[Then(@"My health should be 100")]
        //public void ThenMyHealthShouldBe100()
        //{
        //    Assert.Equal(100, _contex.Player.Health);
        //}

        //[When(@"I take 40 damage")]
        //public void WhenITake40Damage()
        //{
        //    _contex.Player.Hit(40);
        //}

        //[Then(@"My health should now be 60")]
        //public void ThenMyHealthShouldNowBe60()
        //{
        //    Assert.Equal(60, _contex.Player.Health);
        //}

        //[When(@"I take 100 damage")]
        //public void WhenITake100Damage()
        //{
        //    _contex.Player.Hit(100);
        //}

        [Then(@"I should be dead")]
        public void ThenIShouldBeDead()
        {
            Assert.True(_context.Player.IsDead);
        }


        [Given(@"I have a damage resistance of (.*)")]
        public void GivenIHaveADamageResistanceOf(int damageResistance)
        {
            _context.Player.DamageResistance = damageResistance;
        }

        [Given(@"I'm an Elf")]
        public void GivenIMAnElf()
        {
            _context.Player.Race = "Elf";
        }


        [Given(@"I have the following attributes")]
        public void GivenIHaveTheFollowingAttributes(Table table)
        {
            //var race = table.Rows.First(row => row["attribute"] == "Race")["value"];
            //var resistance = table.Rows.First(row => row["attribute"] == "Resistance")["value"];

            // var attributes = table.CreateInstance<PlayerAttributes>();

            dynamic attributes = table.CreateDynamicInstance();//Assist nugeti ile gelen


            //_contex.Player.Race = race;
            _context.Player.Race = attributes.Race;
            //_contex.Player.DamageResistance = int.Parse(resistance);
            _context.Player.DamageResistance = attributes.Resistance;
        }


        [Given(@"My character class is set to (.*)")]
        public void GivenMyCharacterClassİsSetToHealer(CharacterClass characterClass)
        {
            _context.Player.CharacterClass = characterClass;
        }

        [When(@"Cast a healing spell")]
        public void WhenCastAHealingSpell()
        {
            _context.Player.CastHealingSpell();
        }


        [Given(@"I have the following magical items")]
        public void GivenIHaveTheFollowingMagicalİtems(Table table)
        {
            //weakly typed
            //foreach (var row in table.Rows)
            //{
            //    var name = row["item"];
            //    var value = row["value"];
            //    var power = row["power"];

            //    _contex.Player.MagicalItems.Add(new MagicalItem
            //    {
            //        Name = name,
            //        Value = int.Parse(value),
            //        Power = int.Parse(power)
            //    });
            //}

            //strongly typed hali
            //IEnumerable<MagicalItem> items = table.CreateSet<MagicalItem>();
            //_contex.Player.MagicalItems.AddRange(items);

            //dynamic typed
            IEnumerable<dynamic> items = table.CreateDynamicSet();
            foreach (var magicalItem in items)
            {
                _context.Player.MagicalItems.Add(new MagicalItem
                {
                    Name = magicalItem.name,
                    Value = magicalItem.value,
                    Power = magicalItem.power
                });
            }
        }

        [Then(@"My total magical power should be (.*)")]
        public void ThenMyTotalMagicalPowerShouldBe(int expectedPower)
        {
            Assert.Equal(expectedPower, _context.Player.MagicalPower);
        }


        [Given(@"I last slept (.* days ago)")]
        public void GivenILastSleptDaysAgo(DateTime lastSlept)
        {
            _context.Player.LastSleepTime = lastSlept;
        }

        [When(@"I read a restore health scroll")]
        public void WhenIReadARestoreHealthScroll()
        {
            _context.Player.ReadHealthScroll();
        }

        #region Passing data between steps
        [Given(@"I have the following weapons")]
        public void GivenIHaveTheFollowingWeapons(IEnumerable<Weapon> weapons)
        {
            _context.Player.Weapons.AddRange(weapons);
        }

        [Then(@"My weapons should be worth (.*)")]
        public void ThenMyWeaponsShouldBeWorth(int value)
        {
            Assert.Equal(value, _context.Player.WeaponsValue);
        }
        #endregion

        #region Context Injection
        [Given(@"I have an Amulet with a power of (.*)")]
        public void GivenIHaveAnAmuletWithAPoserOf(int power)
        {
            _context.Player.MagicalItems.Add(new MagicalItem
            {
                Name = "Amulet",
                Power = power
            });

            _context.StartingMagicalPower = power;
        }

        [When(@"I use a magical Amulet")]
        public void WhenIUseAMagicalAmulet()
        {
            _context.Player.UseMagicalItem("Amulet");
        }

        [Then(@"The Amulet power should not be reduced")]
        public void ThenTheAmuletPowerShouldNotBeReduced()
        {
            int expectedPower;

            expectedPower = _context.StartingMagicalPower;

            Assert.Equal(expectedPower, _context.Player.MagicalItems.First(item => item.Name == "Amulet").Power);
        }
        #endregion
    }
}
