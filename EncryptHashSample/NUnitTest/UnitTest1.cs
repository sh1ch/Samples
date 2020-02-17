using EncryptHashSample;
using NUnit.Framework;
using System;

namespace NUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("wdDsJwK_uDdCi@")]
        [TestCase("rance_sill_athena")]
        [TestCase("password")]
        [Timeout(50)]
        public void Test_�n�b�V���e�L�X�g�̕������`�F�b�N(string password)
        {
            var hash = HashGenerator.Generate(password);

            // 64 �����ł��邱�Ƃ��`�F�b�N
            Assert.AreEqual(hash.Length, 64);
        }

        [TestCase("wdDsJwK_uDdCi@", 1000)]
        [TestCase("rance_sill_athena", 500)]
        [TestCase("password", 500)]
        public void Test_�n�b�V���e�L�X�g�̍Č����`�F�b�N(string password, int count)
        {
            var hash1 = HashGenerator.Generate(password);

            for (var i = 0; i < count; i ++)
            {
                var hash2 = HashGenerator.Generate(password);

                Assert.AreEqual(hash1, hash2, $"�n�b�V���l����v���܂���Bhash1={hash1}, hash2={hash2}");
            }
        }

        [TestCase("wdDsJwK_uDdCi@", 5)]
        [TestCase("rance_sill_athena", 20)]
        [TestCase("password", 30)]
        public void Test_�n�b�V���l�̕������ύX�`�F�b�N(string password, int size)
        {
            var hash = HashGenerator.Generate(password, size);

            Assert.AreEqual(hash.Length, size);
        }

        [TestCase("", 64)]
        public void Test_�������̃p�X���[�h�̃G���[�`�F�b�N(string password, int size)
        {
            Assert.Throws<ArgumentNullException>(() => 
            {
                var hash = HashGenerator.Generate(password, size);
            });

        }

        [TestCase("password", 0)]
        public void Test_�������̃n�b�V���l�����̃G���[�`�F�b�N(string password, int size)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var hash = HashGenerator.Generate(password, size);
            });

        }

        [TestCase("wdDsJwK_uDdCi@")]
        [TestCase("rance_sill_athena")]
        [TestCase("password")]
        public void Test_16�i���ȊO�̕������܂�ł��Ȃ����̃`�F�b�N(string password)
        {
            var hash = HashGenerator.Generate(password);

            Assert.That(hash, Does.Match("^[0-9|a-f]+$"));
        }
    }
}