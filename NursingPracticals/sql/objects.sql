DROP FUNCTION IF EXISTS fn_check_index_number(VARCHAR(30) ARRAY, OUT BOOLEAN);
CREATE OR REPLACE FUNCTION fn_check_index_number(ids VARCHAR(30) ARRAY, OUT reg BOOLEAN)
RETURNS BOOLEAN AS $$
BEGIN
    reg := (SELECT COUNT(s.indexnumber)= cardinality(ids)
        FROM students s
        WHERE array_positions(ids, s.indexnumber) <> '{}'
    );
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE VIEW vw_class_schedules
AS 
SELECT s.classschedulesid, s.schedulename, s.examdate, s.mainclassesid, c.classname, s.isactive
FROM classschedules s
inner JOIN mainclasses c on c.mainclassesid = s.mainclassesid
WHERE c.classstatus = TRUE;



