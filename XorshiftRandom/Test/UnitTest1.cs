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
        public void Test_�O�ƂP�̕΂肪���X�ł��邩(long seed, long count)
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

            // 0 �� 1 �̊����� 50% �덷�� 0.01 % �ȉ�
            Assert.AreEqual(50.0D, v0 / (double)(v0 + v1) * 100, 0.01D);
            Assert.AreEqual(50.0D, v1 / (double)(v0 + v1) * 100, 0.01D);

            // 0 �� 1 �ȊO�̐������݂��Ȃ�����
            Assert.AreEqual(count, v0 + v1);
        }

        [TestCase(1234567890, 1000 * 1000 * 10, 0, 500)]
        public void Test_�ŏ��l�ƍő�l���o�����邩_����(long seed, long count, int min, int max)
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

            // �ŏ��l�ƍő�l���o�����Ă��邩
            Assert.AreEqual(min, minValue);
            Assert.AreEqual(max, maxValue);
        }

        [TestCase(1234567890, 1000 * 1000 * 10)]
        public void Test_�ŏ��l�ƍő�l���o�����邩_���K(long seed, long count)
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

            // �ŏ��l�ƍő�l�̌덷 0.00001D �ȉ�
            Assert.AreEqual(0.0D, minValue, 0.00001D);
            Assert.AreEqual(1.0D, maxValue, 0.00001D);
        }

        [TestCase(1234567890, 1000 * 1000 * 10, 70.0D)]
        public void Test_�t�@�C�A�[�G���u�����̖�����(long seed, long count, double percent)
        {
            var random = new RandomState();

            var avoidPercent = 100 - percent; // ��������G�̉�𗦂ɕϊ�
            long hit = 0;
            long miss = 0;

            random.SetSeed(seed);

            for (int i = 0; i < count; i++)
            {
                int value = random.Range(0, 255);

                var hitPercent = (value / (double)255) * 100;

                // ����������𗦂��������Ƃ��A�U������������
                if (hitPercent >= avoidPercent) hit += 1;
                else miss += 1;
            }

            // �ŏ��l�ƍő�l�̌덷�� 0.01% �ȉ�
            Assert.AreEqual(percent, hit / (double)(hit + miss) * 100, 0.01D);
            Assert.AreEqual((100 - percent), miss / (double)(hit + miss) * 100, 0.01D);
        }
    }
}