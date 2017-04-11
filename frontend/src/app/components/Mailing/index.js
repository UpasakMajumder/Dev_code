import React from 'react';
import { BrowserRouter as Router, Route } from 'react-router-dom';

import UploadMailList from './UploadMailList';
import MapColumns from './MapColumns';
import MailProcessing from './MailProcessing';

function App() {
  return (
    <Router>
      <div>
        <Route exact path="/new-mailing.html" component={UploadMailList}/>
        <Route path="/map-columns.html" component={MapColumns}/>
        <Route path="/mail-processing.html" component={MailProcessing}/>
      </div>
    </Router>
  );
}

export default App;
