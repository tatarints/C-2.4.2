//Расположения узла относительно родителя
public enum Side
{
    Left,
    Right
}


//Узел дерева
public class TreeNode<T> where T : IComparable
{

    //Конструктор класса
    public TreeNode(T value)
    {
        Value = value;
    }
    //Данные которые хранятся в узле
    public T Value { get; set; }
    //Левая ветка
    public TreeNode<T> LeftNode { get; set; }
    //Правая ветка
    public TreeNode<T> RightNode { get; set; }
    //Родитель
    public TreeNode<T> ParentNode { get; set; }


    //Расположение узла относительно его родителя
    public Side? NodeSide =>
       ParentNode == null
       ? (Side?)null
       : ParentNode.LeftNode == this
           ? Side.Left
           : Side.Right;


    //Преобразование экземпляра класса в строку
    public override string ToString() => Value.ToString();
}


//Дерево
public class Tree<T> where T : IComparable
{

    //Корень дерева
    public TreeNode<T> RootNode { get; set; }

    //Добавление нового узла в дерево
    public TreeNode<T> Add(TreeNode<T> node, TreeNode<T> currentNode = null)
    {
        if (RootNode == null)
        {
            node.ParentNode = null;
            return RootNode = node;
        }

        currentNode = currentNode ?? RootNode;
        node.ParentNode = currentNode;
        int result;
        return (result = node.Value.CompareTo(currentNode.Value)) == 0
          ? currentNode
          : result < 0
              ? currentNode.LeftNode == null
                  ? (currentNode.LeftNode = node)
                  : Add(node, currentNode.LeftNode)
              : currentNode.RightNode == null
                  ? (currentNode.RightNode = node)
                  : Add(node, currentNode.RightNode);
    }
    //Добавление данных в дерево
    public TreeNode<T> Add(T value)
    {
        return Add(new TreeNode<T>(value));
    }



    //Удаление узла дерева по значению
    public void Remove(T value)
    {
        var foundNode = FindNode(value);
        Remove(foundNode);
    }
    //Поиск узла по значению
    public TreeNode<T> FindNode(T value, TreeNode<T> startWithNode = null)
    {
        startWithNode = startWithNode ?? RootNode;
        int result;
        return (result = value.CompareTo(startWithNode.Value)) == 0
         ? startWithNode
         : result < 0
             ? startWithNode.LeftNode == null
                 ? null
                 : FindNode(value, startWithNode.LeftNode)
             : startWithNode.RightNode == null
                 ? null
                 : FindNode(value, startWithNode.RightNode);
    }
    //Удаление узла дерева
    public void Remove(TreeNode<T> node)
    {
        if (node == null)
        {
            return;
        }

        var currentNodeSide = node.NodeSide;
        //если у узла нет подузлов, можно его удалить
        if (node.LeftNode == null && node.RightNode == null)
        {
            if (currentNodeSide == Side.Left)
            {
                node.ParentNode.LeftNode = null;
            }
            else
            {
                node.ParentNode.RightNode = null;
            }
        }
        //если нет левого, то правый ставим на место удаляемого 
        else if (node.LeftNode == null)
        {
            if (currentNodeSide == Side.Left)
            {
                node.ParentNode.LeftNode = node.RightNode;
            }
            else
            {
                node.ParentNode.RightNode = node.RightNode;
            }

            node.RightNode.ParentNode = node.ParentNode;
        }
        //если нет правого, то левый ставим на место удаляемого 
        else if (node.RightNode == null)
        {
            if (currentNodeSide == Side.Left)
            {
                node.ParentNode.LeftNode = node.LeftNode;
            }
            else
            {
                node.ParentNode.RightNode = node.LeftNode;
            }

            node.LeftNode.ParentNode = node.ParentNode;
        }
        //если оба дочерних присутствуют, 
        //то правый становится на место удаляемого,
        //а левый вставляется в правый
        else
        {
            switch (currentNodeSide)
            {
                case Side.Left:
                    node.ParentNode.LeftNode = node.RightNode;
                    node.RightNode.ParentNode = node.ParentNode;
                    Add(node.LeftNode, node.RightNode);
                    break;
                case Side.Right:
                    node.ParentNode.RightNode = node.RightNode;
                    node.RightNode.ParentNode = node.ParentNode;
                    Add(node.LeftNode, node.RightNode);
                    break;
                default:
                    var bufLeft = node.LeftNode;
                    var bufRightLeft = node.RightNode.LeftNode;
                    var bufRightRight = node.RightNode.RightNode;
                    node.Value = node.RightNode.Value;
                    node.RightNode = bufRightRight;
                    node.LeftNode = bufRightLeft;
                    Add(bufLeft, node);
                    break;
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var ITree = new Tree<int>();

        ITree.Add(25);
        ITree.Add(1);
        ITree.Add(20);
        ITree.Add(2);
        ITree.Add(21);
        ITree.Add(10);
        ITree.Add(20);
        ITree.Add(2);
        ITree.Add(41);
    }
}