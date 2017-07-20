import { createStore, applyMiddleware, compose } from 'redux';
import thunkMiddleware from 'redux-thunk';
import { createLogger } from 'redux-logger';
import createHistory from 'history/createBrowserHistory';
import rootReducer from '../reducers';

export const history = createHistory();

export default function configureStore(initialState) {
  /* eslint-disable no-underscore-dangle */
  const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

  if (process.env.NODE_ENV === 'production') {
    return createStore(
      rootReducer,
      initialState,
      composeEnhancers(
        applyMiddleware(thunkMiddleware)
      )
    );
  }

  const loggerMiddleware = createLogger({ collapsed: true, duration: true });

  return createStore(
    rootReducer,
    initialState,
    composeEnhancers(
      applyMiddleware(thunkMiddleware, loggerMiddleware)
    )
  );
  /* eslint-enable */
}
