import { createStore, applyMiddleware, compose } from 'redux';
import thunkMiddleware from 'redux-thunk';
import { createLogger } from 'redux-logger';
import rootReducer from '../reducers';

export default function configureStore(initialState) {
  /* eslint-disable no-underscore-dangle */
  const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

  let middlewares = [thunkMiddleware];

  if (process.env.NODE_ENV === 'development') {
    const loggerMiddleware = createLogger({ collapsed: true, duration: true });
    middlewares = [...middlewares, loggerMiddleware];
  }

  return createStore(
    rootReducer,
    initialState,
    composeEnhancers(applyMiddleware(...middlewares))
  );
  /* eslint-enable */
}
