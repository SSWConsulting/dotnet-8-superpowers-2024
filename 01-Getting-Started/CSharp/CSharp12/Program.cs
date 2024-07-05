//
// Collection Expressions ----------------------------------------------------------------------------------------------
//

// Simple initialization
List<int> numbers = [1, 2, 3, 4, 5];

Foo([1,2,3]);
Foo([]);
Foo2([1,2,3]);
Foo3([1,2,3]);

// Works with IEnumerables
void Foo(IEnumerable<int> numbers)
{
}

// Works with Lists
void Foo2(List<int> numbers)
{
}

// Works with Arrays
void Foo3(int[] numbers)
{
}

//
// Spread Operator -----------------------------------------------------------------------------------------------------
//
List<int> lowNumbers = [1, 2, 3];
List<int> highNumbers = [4, 5, 6];
List<int> allNumbers = [..lowNumbers, ..highNumbers];

Console.ReadKey();

//
// Lambda defaults -----------------------------------------------------------------------------------------------------
//

var lambda = (int start = 0, int end = 10) => Console.WriteLine("Start: {0}, End: {1}", start, end);
lambda();
lambda(2, 5);