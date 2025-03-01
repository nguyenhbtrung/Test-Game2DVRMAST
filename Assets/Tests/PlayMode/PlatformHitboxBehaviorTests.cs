using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HitboxBehaviorTests
{
    public class PlatformHitboxBehaviorTests : HitboxBehaviorTestsBase
    {
        [UnityTest]
        public IEnumerator TestPlayerBlockedByWall()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerBlockedByWall);
            TestLogger.Log(message, logFileName);

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
            string file = $"{TestSettings.SceneName}-{nameof(TestPlayerBlockedByWall)}-InputData";
            LoadSimulatorInputData(file);

            HideTraps();
            HideMovingPlatforms();

            GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
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
                yield return new WaitForSeconds(0.7f);
                Vector3 prePos = player.transform.position;
                yield return new WaitForSeconds(0.2f);
                Vector3 currentPos = player.transform.position;
                if (prePos.x != currentPos.x)
                {
                    string issue = $"Player tại {sequence.begin.position} không bị chặn bởi tường";
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
                string successMessage = $"{nameof(TestPlayerBlockedByWall)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlayerStandOnStaticPlatform()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerStandOnStaticPlatform);
            TestLogger.Log(message, logFileName);

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
            string file = $"{TestSettings.SceneName}-{nameof(TestPlayerStandOnStaticPlatform)}-InputData";
            LoadSimulatorInputData(file);

            HideTraps();
            HideMovingPlatforms();

            GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
            List<string> issues = new();

            foreach (var sequence in sequences)
            {
                player.transform.position = sequence.begin.position;
                cam.transform.position = player.transform.position + Vector3.back * 10;
                ProcessPlayerAction(playerMovement, sequence.begin);

                yield return new WaitForSeconds(0.7f);

                if (player.GetComponent<Rigidbody2D>().velocity.magnitude > 0.01)
                {
                    string issue = $"Player không đứng được trên platform tại {sequence.begin.position} ";
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
                string successMessage = $"{nameof(TestPlayerStandOnStaticPlatform)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlayerStandOnMovingPlatform()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerStandOnMovingPlatform);
            TestLogger.Log(message, logFileName);

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            GameObject parent = GameObject.Find(TestHelper.MOVING_PLATFORM_PARENT_NAME);
            Assert.IsNotNull(parent, "Không tìm thấy MovingPlatformsParent trong scene.");

            GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
            List<string> issues = new();

            foreach (Transform child in parent.transform)
            {
                if (!child.gameObject.name.StartsWith(TestHelper.MOVING_PLATFORM_NAME))
                {
                    continue;
                }
                Vector3 newPos = child.position + Vector3.up * 0.3f;
                player.transform.position = newPos;
                cam.transform.position = player.transform.position + Vector3.back * 10;

                yield return new WaitForSeconds(2);

                float distance = Vector2.Distance(player.transform.position, child.position);

                if ((player.transform.position.y < child.position.y) || (distance > 0.3f))
                {
                    string issue = $"Player không đứng được trên platform động {child.name}. {distance} {player.transform.position} {child.position}";
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
                string successMessage = $"{nameof(TestPlayerStandOnMovingPlatform)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlayerStandUnderVerticalMovingPlatform()
        {
            if (TestHelper.ShouldSkipTest(nameof(TestPlayerStandUnderVerticalMovingPlatform)))
            {
                Assert.Ignore();
            }
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerStandUnderVerticalMovingPlatform);
            TestLogger.Log(message, logFileName);

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
            string file = $"{TestSettings.SceneName}-{nameof(TestPlayerStandUnderVerticalMovingPlatform)}-InputData";
            LoadSimulatorInputData(file);

            HideTraps();

            GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
            List<string> issues = new();

            foreach (var sequence in sequences)
            {
                player.transform.position = sequence.begin.position;
                cam.transform.position = player.transform.position + Vector3.back * 10;
                ProcessPlayerAction(playerMovement, sequence.begin);

                Transform movingPlatform = GetMovingPlatformAbove(sequence.begin.position, maxDistance: 5f);

                if (movingPlatform != null)
                {
                    float initialPlatformY = movingPlatform.position.y;

                    bool hasMovedDown = false;
                    bool hasMovedUpAgain = false;

                    float movementThreshold = 0.1f;

                    while (!hasMovedDown || !hasMovedUpAgain)
                    {
                        float currentPlatformY = movingPlatform.position.y;

                        if (!hasMovedDown && currentPlatformY < initialPlatformY - movementThreshold)
                        {
                            hasMovedDown = true;
                        }

                        if (hasMovedDown && currentPlatformY >= initialPlatformY - movementThreshold)
                        {
                            hasMovedUpAgain = true;
                        }

                        yield return null;
                    }
                }
                yield return new WaitForSeconds(0.5f);

                if (player.transform.position.y - sequence.begin.position.y > 0.2f)
                {
                    string issue = $"Player bị đẩy lên platform động tại {sequence.begin.position}";
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
                string successMessage = $"{nameof(TestPlayerStandUnderVerticalMovingPlatform)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator TestIsGroundedStatusBeforeAndAfterJump()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestIsGroundedStatusBeforeAndAfterJump);
            TestLogger.Log(message, logFileName);

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            dichuyen2 playerMovement = player.GetComponent<dichuyen2>();

            HideTraps();
            HideMovingPlatforms();

            //GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
            List<string> issues = new();

            player.transform.Translate(0.4f, 0, 0);
            yield return new WaitForSeconds(0.5f);

            FieldInfo isGroundedField = typeof(dichuyen2)
                .GetField("isGrounded", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(isGroundedField, "Không tìm thấy trường isGrounded trong PlayerController!");

            bool groundedBefore = (bool)isGroundedField.GetValue(playerMovement);
            if (!groundedBefore)
            {
                issues.Add("IsGrounded = false khi đứng trên mặt đất trước khi nhảy");
            }

            playerMovement.JumpButton();
            yield return null;

            bool groundedAfter = (bool)isGroundedField.GetValue(playerMovement);
            if (groundedAfter)
            {
                issues.Add("IsGrounded = true sau khi nhảy");
            }

            yield return new WaitForSeconds(1f);
            bool groundedWhenGrounding = (bool)isGroundedField.GetValue(playerMovement);
            if (!groundedWhenGrounding)
            {
                issues.Add("IsGrounded = false khi tiếp đất");
            }


            if (issues.Count > 0)
            {
                string errorMessage = string.Join("\n", issues);
                TestLogger.Log(errorMessage, logFileName);
                Assert.Fail(errorMessage);
            }
            else
            {
                string successMessage = $"{nameof(TestIsGroundedStatusBeforeAndAfterJump)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }
    }
}



