const apiRouter = require('./routes');
const bodyParser = require('body-parser');
const express = require('express');
const morgan = require('morgan');
const config = require('../../gulp/config');

const app = express();

app.use(bodyParser.json());
app.use(morgan('dev'));

app.use((req, res, next) => {
  // intercept OPTIONS method
  if (req.method === 'OPTIONS') {
    res.header('Access-Control-Allow-Origin', '*');
    res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE,OPTIONS');
    res.header('Access-Control-Allow-Headers',
      'Content-Type, Authorization, Content-Length, X-Requested-With, X-Redmine-API-Key');

    res.sendStatus(200);
  } else {
    next();
  }
});

app.use('/api', apiRouter);

app.listen(config.MOCK_PORT);

console.log(`App started on port: ${config.MOCK_PORT}`);

module.exports = app;
