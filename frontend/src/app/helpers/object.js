// @flow
export default function removeProps(obj: {}, props: string[]): {} {
  const objRemovedProps: {} = Object.assign({}, obj);
  props.forEach((prop) => { delete objRemovedProps[prop]; });

  return objRemovedProps;
}
