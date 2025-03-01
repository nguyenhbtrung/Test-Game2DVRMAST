using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace HitboxBehaviorTests
{
    public class TrapHitboxBehaviorTests : HitboxBehaviorTestsBase
    {
        [UnityTest]
        public IEnumerator TestPlayerCollisionWithTrapReceiveDamage()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerCollisionWithTrapReceiveDamage);
            TestLogger.Log(message, logFileName);
            List<string> issues = new();

            GameObject trapsParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
            Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

            List<string> trapNames = new();
            foreach (Transform child in trapsParent.transform)
            {
                GameObject trap = child.gameObject;
                TrapType trapType = TestHelper.GetTrapType(trap.name);
                trap = trapType == TrapType.SpikedBall ? TestHelper.GetSpikedBallTrapObject(trap) : trap;
                if (trapType == TrapType.NoDamage || trapType == TrapType.Unknown)
                    continue;
                trapNames.Add(trap.name);
            }
            foreach (string trapName in trapNames)
            {
                SceneManager.LoadScene(TestSettings.SceneIndex);
                yield return null;

                GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
                GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
                Player playerScript = player.GetComponent<Player>();
                GameObject trap = GameObject.Find(trapName);
                if (trap == null)
                    continue;

                int inititalLives = playerScript.lives;

                player.transform.position = trap.transform.position;
                cam.transform.position = player.transform.position + Vector3.back * 10;

                yield return new WaitForFixedUpdate();
                int livesAfterCollision = playerScript.lives;
                int damage = inititalLives - livesAfterCollision;
                int expectedDamage = 1;
                if (damage != expectedDamage)
                {
                    string issue = $"Player va chạm với bẫy {trapName} không nhận đúng thiệt hại. " +
                        $"\nExpected: Damage = {expectedDamage}. " +
                        $"\nActual: Damage = {damage}.";
                    issues.Add(issue);
                }
            }
            if (issues.Count > 0)
            {
                string errorMessage = string.Join("\n", issues);
                TestLogger.Log(errorMessage, logFileName);
                Assert.Fail(errorMessage);
            }
            else
            {
                string successMessage = $"{nameof(TestPlayerCollisionWithTrapReceiveDamage)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlayerCollisionWithTrapDeactivated()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerCollisionWithTrapDeactivated);
            TestLogger.Log(message, logFileName);
            List<string> issues = new();

            GameObject trapsParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
            Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

            List<string> trapNames = new();
            foreach (Transform child in trapsParent.transform)
            {
                GameObject trap = child.gameObject;
                TrapType trapType = TestHelper.GetTrapType(trap.name);
                trap = trapType == TrapType.SpikedBall ? TestHelper.GetSpikedBallTrapObject(trap) : trap;
                if (trapType == TrapType.NoDamage || trapType == TrapType.Unknown)
                    continue;
                trapNames.Add(trap.name);
            }
            foreach (string trapName in trapNames)
            {
                SceneManager.LoadScene(TestSettings.SceneIndex);
                yield return null;

                GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
                GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
                Player playerScript = player.GetComponent<Player>();
                GameObject trap = GameObject.Find(trapName);
                if (trap == null)
                    continue;

                player.transform.position = trap.transform.position;
                cam.transform.position = player.transform.position + Vector3.back * 10;

                yield return new WaitForFixedUpdate();
                if (playerScript.CanMove())
                {
                    string issue = $"Player va chạm với bẫy {trapName} không bị ngừng hoạt động. ";
                    issues.Add(issue);
                }
            }
            if (issues.Count > 0)
            {
                string errorMessage = string.Join("\n", issues);
                TestLogger.Log(errorMessage, logFileName);
                Assert.Fail(errorMessage);
            }
            else
            {
                string successMessage = $"{nameof(TestPlayerCollisionWithTrapDeactivated)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlayerCollisionWithTrapDeadAnimation()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerCollisionWithTrapDeadAnimation);
            TestLogger.Log(message, logFileName);
            List<string> issues = new();

            GameObject trapsParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
            Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

            List<string> trapNames = new();
            foreach (Transform child in trapsParent.transform)
            {
                GameObject trap = child.gameObject;
                TrapType trapType = TestHelper.GetTrapType(trap.name);
                trap = trapType == TrapType.SpikedBall ? TestHelper.GetSpikedBallTrapObject(trap) : trap;
                if (trapType == TrapType.NoDamage || trapType == TrapType.Unknown)
                    continue;
                trapNames.Add(trap.name);
            }
            foreach (string trapName in trapNames)
            {
                SceneManager.LoadScene(TestSettings.SceneIndex);
                yield return null;

                GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
                GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
                Player playerScript = player.GetComponent<Player>();
                GameObject trap = GameObject.Find(trapName);
                if (trap == null)
                    continue;

                player.transform.position = trap.transform.position;
                cam.transform.position = player.transform.position + Vector3.back * 10;

                yield return new WaitForFixedUpdate();
                float duration = 0f;
                bool isDeadAnimation = false;
                while (duration <= 1)
                {
                    if (playerScript.playerSprite.color == playerScript.damagedColor)
                    {
                        isDeadAnimation = true;
                        break;
                    }
                    duration += Time.deltaTime;
                    yield return null;
                }
                if (!isDeadAnimation)
                {
                    string issue = $"Player va chạm với bẫy {trapName} không hiện dead animation. ";
                    issues.Add(issue);
                }
            }
            if (issues.Count > 0)
            {
                string errorMessage = string.Join("\n", issues);
                TestLogger.Log(errorMessage, logFileName);
                Assert.Fail(errorMessage);
            }
            else
            {
                string successMessage = $"{nameof(TestPlayerCollisionWithTrapDeadAnimation)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlayerCollisionWithTrapUntilRunsOutOfLives()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerCollisionWithTrapUntilRunsOutOfLives);
            TestLogger.Log(message, logFileName);
            List<string> issues = new();

            GameObject trapsParent = GameObject.Find(TestHelper.TRAP_PARENT_NAME);
            Assert.IsNotNull(trapsParent, "Không tìm thấy trapsParent trong scene.");

            GameObject selectedTrap = null;
            foreach (Transform child in trapsParent.transform)
            {
                GameObject trap = child.gameObject;
                TrapType trapType = TestHelper.GetTrapType(trap.name);
                trap = trapType == TrapType.SpikedBall ? TestHelper.GetSpikedBallTrapObject(trap) : trap;
                if (trapType == TrapType.NoDamage || trapType == TrapType.Unknown)
                    continue;
                selectedTrap = trap;
                break;
            }

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
            Player playerScript = player.GetComponent<Player>();

            while (playerScript.lives > 0)
            {
                int inititalLives = playerScript.lives;

                player.transform.position = selectedTrap.transform.position;
                cam.transform.position = player.transform.position + Vector3.back * 10;

                yield return new WaitForFixedUpdate();

                int livesAfterCollision = playerScript.lives;
                int damage = inititalLives - livesAfterCollision;
                int expectedDamage = 1;

                if (livesAfterCollision == 0)
                {
                    playerScript.StopAllCoroutines();
                }

                if (damage != expectedDamage)
                {
                    string issue = $"Player va chạm với bẫy không nhận đúng thiệt hại khi còn {inititalLives} mạng. " +
                        $"\nExpected: Damage = {expectedDamage}. " +
                        $"\nActual: Damage = {damage}.";
                    issues.Add(issue);
                }
                if (playerScript.CanMove())
                {
                    string issue = $"Player va chạm với bẫy không bị ngừng hoạt động khi còn {inititalLives} mạng. ";
                    issues.Add(issue);
                }

                float duration = 0f;
                bool isDeadAnimation = false;
                while (duration <= 1)
                {
                    if (playerScript.playerSprite.color == playerScript.damagedColor)
                    {
                        isDeadAnimation = true;
                        break;
                    }
                    duration += Time.deltaTime;
                    yield return null;
                }
                if (!isDeadAnimation)
                {
                    string issue = $"Player va chạm với bẫy không hiện dead animation khi còn {inititalLives} mạng. ";
                    issues.Add(issue);
                }
                yield return new WaitForSeconds(2);
            }

            if (issues.Count > 0)
            {
                string errorMessage = string.Join("\n", issues);
                TestLogger.Log(errorMessage, logFileName);
                Assert.Fail(errorMessage);
            }
            else
            {
                string successMessage = $"{nameof(TestPlayerCollisionWithTrapUntilRunsOutOfLives)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlayerCollisionWithNoDamegeTrap()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerCollisionWithNoDamegeTrap);
            TestLogger.Log(message, logFileName);

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
            string file = $"{TestSettings.SceneName}-{nameof(TestPlayerCollisionWithNoDamegeTrap)}-InputData";
            LoadSimulatorInputData(file);

            HideTraps(true);
            HideMovingPlatforms();

            GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
            List<string> issues = new();

            foreach (var sequence in sequences)
            {
                player.transform.position = sequence.begin.position;
                cam.transform.position = player.transform.position + Vector3.back * 10;
                ProcessPlayerAction(playerMovement, sequence.begin);
                yield return new WaitForSeconds(0.2f);
                Vector3 prePos = player.transform.position;

                yield return new WaitForSeconds(1.5f);

                if (player.transform.position == prePos)
                {
                    string issue = $"Player bị xuyên qua NoDamageTrap tại {sequence.begin.position} ";
                    issues.Add(issue);
                }

            }

            if (issues.Count > 0)
            {
                string errorMessage = string.Join("\n", issues);
                TestLogger.Log(errorMessage, logFileName);
                Assert.Fail(errorMessage);
            }
            else
            {
                string successMessage = $"{nameof(TestPlayerCollisionWithNoDamegeTrap)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }
    }
}


