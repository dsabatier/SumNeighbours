using System;
using System.Collections.Generic;

namespace SumNeighbours
{
    public class MoveHistory
    {
        public Stack<Move> _moveHistory = new Stack<Move>();
        public void AddMove(Node node, int oldValue, int newValue)
        {
            Move m = new Move(node, oldValue, newValue);
            _moveHistory.Push(m);
        }

        public Move Undo()
        {
            if (_moveHistory.Count > 0)
                return _moveHistory.Pop()?.Undo();

            return Move.NoMove;
        }
    }

    public class Move
    {
        public static Move NoMove = new NullMove();
        public readonly Node Node;
        private readonly int _oldValue;
        private readonly int _newValue;

        protected Move()
        {
            Node = Node.NoNode;
            _oldValue = 0;
            _newValue = 0;
        }

        public Move(Node node, int oldValue, int newValue)
        {
            Node = node;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public virtual Move Undo()
        {
            Node.SetValue(_oldValue);
            return this;
        }
    }

    public class NullMove : Move
    {
        public override Move Undo()
        {
            return this;
        }
    }
}