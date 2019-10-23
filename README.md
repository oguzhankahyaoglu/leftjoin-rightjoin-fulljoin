# joinextensions-iqueryable
Very simple Left/Right/FullOuterJoin extension methods for both IQueryable and IEnumerable types

In linq, if you need a left outer join, you have to use GroupJoin/Selectmany/DefaultIfEmpty blocks. There is another package in nuget containing similar extensions for only IEnumerable, which results in performance loss for database queries. For this need, I developed IQueryable versions. 

Functionalities:

- Left Join for IQueryables: Translated into database queries
- Left Join for IEnumerables: In memory joining
- Left Excluding Join for IQueryable/IEnumerables: Taking only Left side for matching items
- Right Join for IQueryable
- Right Join for IEnumerable
- Right Excluding Join for IQueryable/IEnumerables
- Full Outer Join for IQueryable
- Full Outer Join for IEnumerable
- Left Join for IEnumerable via ON statement condition: other overloads uses lambdas for **LEFT=> key** and **RIGTH => key** syntax, this overload uses **(LEFT, RIGHT) => boolean** syntax
