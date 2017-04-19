import React from 'react';
import { Route } from 'react-router-dom';
import { ConnectedRouter } from 'react-router-redux';

import UploadMailList from './UploadMailList';
import MapColumns from './MapColumns';
import MailProcessing from './MailProcessing';
import { history } from '../../store';

function App() {
  return (
    <ConnectedRouter history={history}>
      <div>
        <Route exact path="/new-mailing.html" component={UploadMailList}/>
        <Route path="/map-columns.html" component={MapColumns}/>
        <Route path="/mail-processing.html" component={MailProcessing}/>
      </div>
    </ConnectedRouter>
  );
}

export default App;
