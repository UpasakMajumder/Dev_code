// @flow
import React, { Component, type Node } from 'react';

type Props = {
  className: string,
  isActive: boolean,
  children?: Node
};

class BodyClassToggler extends Component<Props> {
  bodyEl: HTMLBodyElement

  constructor() {
    super();

    this.bodyEl = ((document.body: any): HTMLBodyElement);
  }

  componentWillReceiveProps(nextProps: Props) {
    if (this.props.isActive && !nextProps.isActive) {
      this.remove();
    } else if (!this.props.isActive && nextProps.isActive) {
      this.add();
    }
  }

  componentDidMount() {
    this.props.isActive ? this.add() : this.remove();
  }

  componentWillUnmount() {
    this.remove();
  }

  add = () => {
    this.bodyEl.classList.add(this.props.className);
  }

  remove = () => {
    this.bodyEl.classList.remove(this.props.className);
  }

  render() {
    return (this.props.children);
  }
}

export default BodyClassToggler;
