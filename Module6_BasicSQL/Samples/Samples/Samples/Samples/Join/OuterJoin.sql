SELECT *
FROM JoinSamples.Employee as e
LEFT OUTER JOIN JoinSamples.Department as d ON e.DepartmentID = d.Id

SELECT *
FROM JoinSamples.Employee as e
LEFT JOIN JoinSamples.Department as d ON e.DepartmentID = d.Id



SELECT *
FROM JoinSamples.Employee as e
RIGHT OUTER JOIN JoinSamples.Department as d ON e.DepartmentID = d.Id

SELECT *
FROM JoinSamples.Employee as e
RIGHT JOIN JoinSamples.Department as d ON e.DepartmentID = d.Id




SELECT *
FROM JoinSamples.Employee as e
FULL OUTER JOIN JoinSamples.Department as d ON e.DepartmentID = d.Id


SELECT *
FROM JoinSamples.Employee as e
FULL JOIN JoinSamples.Department as d ON e.DepartmentID = d.Id