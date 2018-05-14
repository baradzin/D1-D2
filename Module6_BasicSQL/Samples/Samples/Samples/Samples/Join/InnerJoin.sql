SELECT *
FROM JoinSamples.Employee as e
INNER JOIN JoinSamples.Department as d ON e.DepartmentID = d.Id


SELECT *
FROM JoinSamples.Employee as e
JOIN JoinSamples.Department as d ON e.DepartmentID = d.Id



SELECT *
FROM JoinSamples.Employee as e, JoinSamples.Department as d
WHERE e.DepartmentID = d.Id