import isEqual from 'lodash/isEqual';

// @flow
export function removeProps(obj: {}, props: string[]): {} {
  const objRemovedProps: {} = Object.assign({}, obj);
  props.forEach((prop) => { delete objRemovedProps[prop]; });

  return objRemovedProps;
}

/**
 * @param obj1 object
 * @param obj2 object
 * @param array resulted array
 * Use as Partial Application with array/compareArrays
 */

export const findInequals = (obj1, obj2, array) => {
  if (!isEqual(obj1, obj2)) array.push(obj1);
};
