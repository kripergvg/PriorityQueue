using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace PriorityQueue
{
    [TestFixture]
    public class PriorityQueueTests
    {
        [Test]
        public void Dequeue_WhenEnqueueInOrder_ShouldDequeueInOrder()
        {
            // SUT
            var queue = new PriorityQueue<int, int>();
            queue.Enqueue(1, 1);
            queue.Enqueue(2, 2);
            queue.Enqueue(3, 3);
            queue.Enqueue(4, 4);
            queue.Enqueue(5, 5);
            queue.Enqueue(6, 6);
            
            // when
            var values = DequeueAsFlat(queue);
            
            // then
            values[0].Should().Be(1);
            values[1].Should().Be(2);
            values[2].Should().Be(3);
            values[3].Should().Be(4);
            values[4].Should().Be(5);
            values[5].Should().Be(6);
        }
        
        [Test]
        public void Dequeue_WhenEnqueueNotInOrder_ShouldDequeueInOrder()
        {
            // SUT
            var queue = new PriorityQueue<int, int>();
            queue.Enqueue(1, 1);
            queue.Enqueue(6, 6);
            queue.Enqueue(2, 2);
            queue.Enqueue(4, 4);
            queue.Enqueue(3, 3);
            queue.Enqueue(5, 5);
            
            // when
            var values = DequeueAsFlat(queue);
            
            // then
            values[0].Should().Be(1);
            values[1].Should().Be(2);
            values[2].Should().Be(3);
            values[3].Should().Be(4);
            values[4].Should().Be(5);
            values[5].Should().Be(6);
        }

        [Test]
        public void Dequeue_WhenDuplicateValue_ShouldReturnDuplicatedCorrectly()
        {
            // SUT
            var queue = new PriorityQueue<int, int>();
            queue.Enqueue(20, 20);
            queue.Enqueue(21, 21);
            queue.Enqueue(19, 19);
            queue.Enqueue(18, 18);
            queue.Enqueue(25, 25);
            queue.Enqueue(19, 19);
            queue.Enqueue(19, 19);
            queue.Enqueue(19, 19);
            queue.Enqueue(19, 19);
            
            // when
            var values = DequeueAsFlat(queue);
            
            // then
            values[0].Should().Be(18);
            values[1].Should().Be(19);
            values[2].Should().Be(19);
            values[3].Should().Be(19);
            values[4].Should().Be(19);
            values[5].Should().Be(19);
            values[6].Should().Be(20);
            values[7].Should().Be(21);
            values[8].Should().Be(25);
        }

        private List<int> DequeueAsFlat(PriorityQueue<int, int> queue)
        {
            var values = new List<int>();
            while (queue.TryDequeue(out var newValue))
            {
                values.Add(newValue);
            }

            return values;
        }
    }
}