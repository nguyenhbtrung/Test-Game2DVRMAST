using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace HitboxBehaviorTests
{
    public class FinishPointBehaviorTests : HitboxBehaviorTestsBase
    {
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Time.timeScale = 1;
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlayerCollisionFinishPoint()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerCollisionFinishPoint);
            TestLogger.Log(message, logFileName);

            GameObject finishPoint = GameObject.Find(TestHelper.FINISH_POINT_NAME);
            Assert.IsNotNull(finishPoint, "Không tìm thấy FinishPoint trong scene.");
            Win win = finishPoint.GetComponent<Win>();

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);

            player.transform.position = finishPoint.transform.position;
            yield return null;

            if (!win.gameWinIU.activeInHierarchy)
            {
                string err = "Màn hình gameWin không hiển thị khi Player va chạm với FinishPoint.";
                TestLogger.Log(err, logFileName);
                Assert.Fail(err);
            }

            string successMessage = $"{nameof(TestPlayerCollisionFinishPoint)} passed.";
            TestLogger.Log(successMessage, logFileName);
            Assert.Pass(successMessage);
            Time.timeScale = 1;

            yield return null;
        }


        [UnityTest]
        public IEnumerator TestPlayerMovingLeftHitFinishPoint()
        {
            if (TestHelper.ShouldSkipTest(nameof(TestPlayerMovingLeftHitFinishPoint)))
            {
                Assert.Ignore();
            }
            yield return TestPlayerHitFinishPointWhenMove
            (
                testName: nameof(TestPlayerMovingLeftHitFinishPoint),
                inputDataName: nameof(TestPlayerMovingLeftHitFinishPoint),
                CheckAction: CheckGameWin
            );
        }

        [UnityTest]
        public IEnumerator TestPlayerMovingRightHitFinishPoint()
        {
            if (TestHelper.ShouldSkipTest(nameof(TestPlayerMovingRightHitFinishPoint)))
            {
                Assert.Ignore();
            }
            yield return TestPlayerHitFinishPointWhenMove
            (
                testName: nameof(TestPlayerMovingRightHitFinishPoint),
                inputDataName: nameof(TestPlayerMovingRightHitFinishPoint),
                CheckAction: CheckGameWin
            );
        }

        [UnityTest]
        public IEnumerator TestPlayerMovingLeftAndJumpHitFinishPoint()
        {
            if (TestHelper.ShouldSkipTest(nameof(TestPlayerMovingLeftAndJumpHitFinishPoint)))
            {
                Assert.Ignore();
            }
            yield return TestPlayerHitFinishPointWhenMove
            (
                testName: nameof(TestPlayerMovingLeftAndJumpHitFinishPoint),
                inputDataName: nameof(TestPlayerMovingLeftAndJumpHitFinishPoint),
                CheckAction: CheckGameWin
            );
        }

        [UnityTest]
        public IEnumerator TestPlayerMovingRightAndJumpHitFinishPoint()
        {
            if (TestHelper.ShouldSkipTest(nameof(TestPlayerMovingRightAndJumpHitFinishPoint)))
            {
                Assert.Ignore();
            }
            yield return TestPlayerHitFinishPointWhenMove
            (
                testName: nameof(TestPlayerMovingRightAndJumpHitFinishPoint),
                inputDataName: nameof(TestPlayerMovingRightAndJumpHitFinishPoint),
                CheckAction: CheckGameWin
            );
        }

        public IEnumerator CheckGameWin(Win win, List<string> issues)
        {

            yield return new WaitForFixedUpdate();
            if (!win.gameWinIU.activeInHierarchy)
            {
                string issue = "Màn hình gameWin không hiển thị khi Player va chạm với FinishPoint.";
                issues.Add(issue);
            }
            
        }

        public IEnumerator TestPlayerHitFinishPointWhenMove(string testName, string inputDataName, CheckActionDelegate CheckAction)
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = testName;
            TestLogger.Log(message, logFileName);

            string file = $"{TestSettings.SceneName}-{inputDataName}-InputData";
            LoadSimulatorInputData(file);

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
            Player playerScript = player.GetComponent<Player>();
            GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
            CollisionDetector collisionDetector = player.AddComponent<CollisionDetector>();
            Win win = GameObject.Find(TestHelper.FINISH_POINT_NAME).GetComponent<Win>();
            HideMovingPlatforms();

            List<string> issues = new();

            foreach (var sequence in sequences)
            {

                player.transform.position = sequence.begin.position;
                cam.transform.position = player.transform.position + Vector3.back * 10;
                ProcessPlayerAction(playerMovement, sequence.begin);
                if (sequence.actions != null)
                    foreach (var actionData in sequence.actions)
                    {
                        yield return new WaitUntil(() =>
                            Vector2.Distance(player.transform.position, actionData.position) < positionThreshold);
                        ProcessPlayerAction(playerMovement, actionData);

                    }
                float startTime = Time.time;
                yield return new WaitUntil(() =>
                            collisionDetector.IsCollisionTrap ||
                            Time.time - startTime >= 2f);
                yield return CheckAction(win, issues);

            }

            if (issues.Count > 0)
            {
                string errorMessage = string.Join("\n", issues);
                TestLogger.Log(errorMessage, logFileName);
                Assert.Fail(errorMessage);
            }
            else
            {
                string successMessage = $"{testName} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }

        public delegate IEnumerator CheckActionDelegate(Win win, List<string> issues);
    }
}

