using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace HitboxBehaviorTests
{
    public class CheckpointHitboxBehaviorTests : HitboxBehaviorTestsBase
    {
        [UnityTest]
        public IEnumerator TestPlayerAlwaysHitsCameraCheckpointAtAreaIntersections()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerAlwaysHitsCameraCheckpointAtAreaIntersections);
            TestLogger.Log(message, logFileName);

            
            string file = $"{TestSettings.SceneName}-{nameof(TestPlayerAlwaysHitsCameraCheckpointAtAreaIntersections)}-InputData";
            LoadSimulatorInputData(file);

            List<string> issues = new();

            foreach (var sequence in sequences)
            {
                SceneManager.LoadScene(TestSettings.SceneIndex);
                yield return null;
                HideTraps();
                HideMovingPlatforms();
                GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
                dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
                player.GetComponent<Rigidbody2D>().isKinematic = true;
                GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;
                Vector3 camPrePos = cam.transform.position;

                player.transform.position = sequence.begin.position;
                yield return new WaitForSeconds(0.2f);

                if (cam.transform.position == camPrePos)
                {
                    string issue = $"Player tại vị trí {sequence.begin.position} không chạm camera checkpoint";
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
                string successMessage = $"{nameof(TestPlayerAlwaysHitsCameraCheckpointAtAreaIntersections)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }


        [UnityTest]
        public IEnumerator TestPlayerAlwaysHitsPlayerCheckpoint()
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = nameof(TestPlayerAlwaysHitsPlayerCheckpoint);
            TestLogger.Log(message, logFileName);


            string file = $"{TestSettings.SceneName}-{nameof(TestPlayerAlwaysHitsPlayerCheckpoint)}-InputData";
            LoadSimulatorInputData(file);

            List<string> issues = new();

            foreach (var sequence in sequences)
            {
                SceneManager.LoadScene(TestSettings.SceneIndex);
                yield return null;
                
                HideMovingPlatforms();
                HideCameraCheckpoints();

                GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
                dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
                player.GetComponent<Rigidbody2D>().isKinematic = true;
                CollisionDetector collisionDetector = player.AddComponent<CollisionDetector>();
                GameObject cam = GameObject.FindObjectOfType<Camera>().gameObject;

                player.transform.position = sequence.begin.position;
                cam.transform.position = player.transform.position + Vector3.back * 10f;
                yield return new WaitForSeconds(0.2f);

                player.transform.position = sequence.actions[0].position;
                cam.transform.position = player.transform.position + Vector3.back * 10f;
                player.GetComponent<Rigidbody2D>().isKinematic = false;

                yield return new WaitForSeconds(1f);
                cam.transform.position = (Vector3)collisionDetector.CheckPointPos + Vector3.back * 10;
                yield return new WaitForSeconds(1f);

                if (Vector2.Distance(player.transform.position, collisionDetector.CheckPointPos) > 0.3f)
                {
                    string issue = $"Player tại vị trí {sequence.begin.position} không chạm Player checkpoint.";
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
                string successMessage = $"{nameof(TestPlayerAlwaysHitsPlayerCheckpoint)} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }

        //[UnityTest]
        //public IEnumerator TestPlayerMoveLeftHitCameraCheckpoint()
        //{
        //    yield return TestPlayerMoveHitCameraCheckpoint
        //    (
        //        nameof(TestPlayerMoveLeftHitCameraCheckpoint)
        //    );
        //}

        public IEnumerator TestPlayerMoveHitCameraCheckpoint(string testName)
        {
            string logFileName = TestSettings.TestLogFileName;
            string message = testName;
            TestLogger.Log(message, logFileName);

            string file = $"{TestSettings.SceneName}-{testName}-InputData";
            LoadSimulatorInputData(file);

            GameObject player = GameObject.Find(TestHelper.PLAYER_NAME);
            dichuyen2 playerMovement = player.GetComponent<dichuyen2>();
            Camera cam = GameObject.FindObjectOfType<Camera>();
            HideTraps();
            HideMovingPlatforms();
            List<string> issues = new();

            foreach (var sequence in sequences)
            {
                Vector3 camPrePos = cam.transform.position;
                player.transform.position = sequence.begin.position;
                ProcessPlayerAction(playerMovement, sequence.begin);
                if (sequence.actions != null)
                    foreach (var actionData in sequence.actions)
                    {
                        yield return new WaitUntil(() =>
                            Vector2.Distance(player.transform.position, actionData.position) < positionThreshold);
                        ProcessPlayerAction(playerMovement, actionData);
                    }
                yield return new WaitForSeconds(1f);
                Vector3 viewportPoint = cam.WorldToViewportPoint(player.transform.position);
                bool isPlayerVisibleInScreen = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z >= 0;
                bool isCamMove = cam.transform.position != camPrePos;

                if (!isPlayerVisibleInScreen || !isCamMove)
                {
                    string issue = $"Camera không di chuyển khi Player đi qua Camera Checkpoint tại {sequence.begin.position}";
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
                string successMessage = $"{testName} passed.";
                TestLogger.Log(successMessage, logFileName);
                Assert.Pass(successMessage);
            }

            yield return null;
        }
    }
}

