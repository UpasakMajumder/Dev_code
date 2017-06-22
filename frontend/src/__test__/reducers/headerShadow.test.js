import reducer from '../../app/reducers/headerShadow';
import { HEADER_SHADOW_HIDE, HEADER_SHADOW_SHOW } from '../../app/constants';

describe('Header shadow reducer', () => {
  function getActualState(defaultState, actionType) {
    return reducer(defaultState, { type: actionType });
  }

  test('Should be shown', () => {
    const actual = getActualState(false, HEADER_SHADOW_SHOW);
    const expected = true;
    expect(actual).toEqual(expected);
  });

  test('Should be hidden', () => {
    const actual = getActualState(true, HEADER_SHADOW_HIDE);
    const expected = false;
    expect(actual).toEqual(expected);
  });
});
