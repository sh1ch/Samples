using NUnit.Framework;
using System;
using XorshiftRandom;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(637208974768768269, 1000*1000*10)]
        public void Test_０と１の偏りが半々であるか(long seed, long count)
        {
            var random = new RandomState();

            long v0 = 0;
            long v1 = 0;

            random.SetSeed(seed);

            for (int i = 0; i < count; i++)
            {
                var value = random.Range(0, 1);

                if (value == 0) v0 += 1;
                if (value == 1) v1 += 1;
            }

            // 0 と 1 の割合が 50% 誤差率 0.01 % 以下
            Assert.AreEqual(50.0D, v0 / (double)(v0 + v1) * 100, 0.01D);
            Assert.AreEqual(50.0D, v1 / (double)(v0 + v1) * 100, 0.01D);

            // 0 と 1 以外の数が存在しないこと
            Assert.AreEqual(count, v0 + v1);
        }

        [TestCase(1234567890, 1000 * 1000 * 10, 0, 500)]
        public void Test_最小値と最大値が出現するか_整数(long seed, long count, int min, int max)
        {
            var random = new RandomState();

            int minValue = max;
            int maxValue = min;

            random.SetSeed(seed);

            for (int i = 0; i < count; i++)
            {
                var value = random.Range(min, max);

                if (minValue > value) minValue = value;
                if (maxValue < value) maxValue = value;
            }

            // 最小値と最大値が出現しているか
            Assert.AreEqual(min, minValue);
            Assert.AreEqual(max, maxValue);
        }

        [TestCase(1234567890, 1000 * 1000 * 10)]
        public void Test_最小値と最大値が出現するか_正規(long seed, long count)
        {
            var random = new RandomState();

            double minValue = 1.0D;
            double maxValue = 0.0D;

            random.SetSeed(seed);

            for (int i = 0; i < count; i++)
            {
                double value = random.Range(0.0D, 1.0D);

                if (minValue > value) minValue = value;
                if (maxValue < value) maxValue = value;
            }

            // 最小値と最大値の誤差 0.00001D 以下
            Assert.AreEqual(0.0D, minValue, 0.00001D);
            Assert.AreEqual(1.0D, maxValue, 0.00001D);
        }

        [TestCase(1234567890, 1000 * 1000 * 10, 70.0D)]
        public void Test_ファイアーエムブレムの命中率(long seed, long count, double percent)
        {
            var random = new RandomState();

            var avoidPercent = 100 - percent; // 命中率を敵の回避率に変換
            long hit = 0;
            long miss = 0;

            random.SetSeed(seed);

            for (int i = 0; i < count; i++)
            {
                int value = random.Range(0, 255);

                var hitPercent = (value / (double)255) * 100;

                // 命中率が回避率を上回ったとき、攻撃が成立する
                if (hitPercent >= avoidPercent) hit += 1;
                else miss += 1;
            }

            // 最小値と最大値の誤差率 0.01% 以下
            Assert.AreEqual(percent, hit / (double)(hit + miss) * 100, 0.01D);
            Assert.AreEqual((100 - percent), miss / (double)(hit + miss) * 100, 0.01D);
        }
    }
}