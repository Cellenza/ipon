import React, { Component } from 'react';
import { Button } from 'react-bootstrap';

export class Home extends Component {
  displayName = Home.name

  constructor(props) {
      super(props);

    this.turnOn = this.turnOn.bind(this);
    this.turnOff = this.turnOff.bind(this);
  }

  async turnOn() {
    await fetch('pinpon/TurnOn');
  }

  async turnOff() {
    await fetch('pinpon/TurnOff');
  }

  render() {
    return (
      <div>
        <h1>Pinpon Manager</h1>
        <p>Activer le pinpon</p>
        <Button bsStyle="success" onClick={this.turnOn}>Allumer</Button>
        <Button onClick={this.turnOff}>Eteindre</Button>
      </div>
    );
  }
}
