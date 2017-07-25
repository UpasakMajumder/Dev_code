export default function removeProps(obj, props) {
  const objRemovedProps = Object.assign({}, obj);
  props.forEach((prop) => { delete objRemovedProps[prop]; });

  return objRemovedProps;
}
