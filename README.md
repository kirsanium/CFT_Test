# CFT_Test

### Setting up

`docker-compose build && docker-compose up`
to deploy the application.

### General usage
Send **POST** requests to add data, **PUT** requests to change data, **GET** requests to retrieve data, and **DELETE** to delete data.

To filter or sort data in your tasks **GET** request use following query syntax:
`localhost:5000/tasks?parameter=value`, where `values` and possible `parameters` are:
- startdate: date in ddMMyyyy format
- enddate: date in ddMMyyyy format
- status: [new|inprogress|closed]
- priority: 0 to 255
- datesort: asc for ascending sort, desc for descending
- prioritysort: asc for ascending sort, desc for descending
- You can't sort by both parameters simultaneously.
