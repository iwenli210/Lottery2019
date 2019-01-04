﻿using FlysEngine.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lottery2019.UI.Sprites
{
    public static class SpriteQueryExtensions
    {
        public static IEnumerable<Sprite> FindAll(this IEnumerable<Sprite> sprites, string spriteType)
        {
            foreach (var sprite in sprites)
            {
                if (sprite.SpriteType == spriteType)
                {
                    yield return sprite;
                }

                foreach (var childSprite in FindAll(sprite.Children, spriteType))
                {
                    yield return childSprite;
                }
            }
        }

        public static Sprite FindSingle(this IEnumerable<Sprite> sprites, string spriteType)
        {
            return FindAll(sprites, spriteType).Single();
        }

        public static T QueryBehavior<T>(this IEnumerable<Sprite> sprites, string spriteType)
            where T : Behavior
        {
            var sprite = sprites.FindSingle(spriteType);
            return sprite.Behaviors.QueryBehavior<T>();
        }

        public static T QueryBehavior<T>(this Dictionary<string, Behavior> behaviors)
            where T : Behavior
        {
            var behavior = behaviors[typeof(T).Name];
            return (T)behavior;
        }

        public static void QueryBehaviorAll<T>(this IEnumerable<Sprite> sprites, string spriteType, Action<T> action)
            where T : Behavior
        {
            var all = sprites.FindAll(spriteType);
            foreach (var sprite in all)
            {
                action(sprite.Behaviors.QueryBehavior<T>());
            }
        }

        public static bool RemoveChild(this List<Sprite> sprites, Sprite val)
        {
            if (sprites.Remove(val))
                return true;

            foreach (var sprite in sprites)
            {
                if (sprite.Children.RemoveChild(val))
                    return true;
            }

            return false;
        }
    }
}
