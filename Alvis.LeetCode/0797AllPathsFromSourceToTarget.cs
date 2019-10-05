using System.Collections.Generic;

partial class Solution
{
    public IList<IList<int>> AllPathsSourceTarget(int[][] graph)
    {
        var target = graph.Length - 1;
        var paths = new List<IList<int>>(graph.Length);
        CollectPaths(new List<int>(graph.Length), 0);
        return paths;

        void CollectPaths(List<int> context, int nodeIndex)
        {
            var nextNodeIndices = graph[nodeIndex];
            context.Add(nodeIndex);
            for (var i = 0; i < nextNodeIndices.Length; i++)
            {
                var nextNodeIndex = nextNodeIndices[i];
                if (nextNodeIndex != target)
                    CollectPaths(context, nextNodeIndex);
                else
                {
                    var path = new int[context.Count + 1];
                    context.CopyTo(path, 0);
                    path[path.Length - 1] = nextNodeIndex;
                    paths.Add(path);
                }
            }
            context.RemoveAt(context.Count - 1);
        }
    }
}

partial class Tests
{
    [NUnit.Framework.Test(Description = "https://leetcode.com/problems/all-paths-from-source-to-target/")]
    [NUnit.Framework.TestCase("[[1,2],[3],[3],[]]", ExpectedResult = "[[0,1,3],[0,2,3]]")]
    public object AllPathsSourceTarget(params object[] args) => InvokeTest().IsUnordered();
}
