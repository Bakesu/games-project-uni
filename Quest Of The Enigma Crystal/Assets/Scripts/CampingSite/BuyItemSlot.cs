using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.Scripts.CampingSite
{
    public class BuyItemSlot : MonoBehaviour
    {
        static class RandomNumbers
        {
            private static Random random = new Random();

            public static int GetRandom(int min, int max)
            {
                return random.Next(min, max + 1);
            }
        }
        void Start()
        {
            List<string> unitNames = new List<string>();

            Sprite apprenticeSprite = Resources.Load<Sprite>("Sprites/apprentice_sprite");
            Sprite clericSprite = Resources.Load<Sprite>("Sprites/cleric_sprite");
            Sprite highpriestSprite = Resources.Load<Sprite>("Sprites/highpriest_sprite");
            Sprite enchanterSprite = Resources.Load<Sprite>("Sprites/enchanter_sprite");
            Sprite sorcererSprite = Resources.Load<Sprite>("Sprites/sorcerer_sprite");

            Sprite pikemanSprite = Resources.Load<Sprite>("Sprites/pikeman_sprite");
            Sprite infantrymanSprite = Resources.Load<Sprite>("Sprites/infantryman_sprite");
            Sprite paladinSprite = Resources.Load<Sprite>("Sprites/paladin_sprite");
            Sprite championSprite = Resources.Load<Sprite>("Sprites/champion_sprite");
            Sprite archerSprite = Resources.Load<Sprite>("Sprites/archer_sprite");
            Sprite arbalistSprite = Resources.Load<Sprite>("Sprites/arbalist_sprite");

            for (int i = 0; i < 10; i++)
            {
                unitNames.Add("Pikeman shop");
            }
            for (int i = 0; i < 3; i++)
            {
                unitNames.Add("Infantryman shop");
            }
            unitNames.Add("Paladin shop");
            unitNames.Add("Champion shop");

            for (int i = 0; i < 3; i++)
            {
                unitNames.Add("Archer shop");
            }
            unitNames.Add("Arbalist shop");

            for (int i = 0; i < 10; i++)
            {
                unitNames.Add("Apprentice shop");
            }
            for (int i = 0; i < 3; i++)
            {
                unitNames.Add("Enchanter shop");
            }
            for (int i = 0; i < 3; i++)
            {
                unitNames.Add("Cleric shop");
            }
            unitNames.Add("Cleric shop");
            unitNames.Add("HighPriest shop");

            int RNG = RandomNumbers.GetRandom(0, unitNames.Count + 5);

            if (transform.childCount <= 0 && RNG < unitNames.Count)
            {
                GameObject unit = new GameObject(unitNames[RNG]);

                unit.AddComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;

                unit.AddComponent<DragDrop>();

                RectTransform rect = unit.GetComponent<RectTransform>();
                rect.SetParent(transform);
                rect.localPosition = new Vector3(1, 1, 1);
                rect.localScale = new Vector3(1, 1, 1);

                unit.SetActive(true);

                unit.AddComponent<CanvasGroup>();
                unit.AddComponent<CanvasRenderer>();

                if (unit.name.Contains("Pikeman"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = pikemanSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 5;
                    unitGold.buyUnitGold = 10;

                }
                else if (unit.name.Contains("Infantryman"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = infantrymanSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 15;
                    unitGold.buyUnitGold = 30;

                }
                else if (unit.name.Contains("Paladin"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = paladinSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 50;
                    unitGold.buyUnitGold = 100;

                }
                else if (unit.name.Contains("Champion"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = championSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 50;
                    unitGold.buyUnitGold = 100;

                }
                else if (unit.name.Contains("Archer"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = archerSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 15;
                    unitGold.buyUnitGold = 30;

                }
                else if (unit.name.Contains("Arbalist"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = arbalistSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 50;
                    unitGold.buyUnitGold = 100;

                }
                else if (unit.name.Contains("Apprentice"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = apprenticeSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 5;
                    unitGold.buyUnitGold = 10;

                }
                else if (unit.name.Contains("Enchanter"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = enchanterSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 15;
                    unitGold.buyUnitGold = 30;

                }
                else if (unit.name.Contains("Sorcerer"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = sorcererSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 50;
                    unitGold.buyUnitGold = 100;

                }
                else if (unit.name.Contains("Cleric"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = clericSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 15;
                    unitGold.buyUnitGold = 30;

                }
                else if (unit.name.Contains("HighPriest"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = highpriestSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = 50;
                    unitGold.buyUnitGold = 100;

                }

            }
        }
    }
}
