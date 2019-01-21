import React, { Component } from 'react';
import { Table, Button, Glyphicon } from 'react-bootstrap';
import QueueModal from './QueueModal';

export class Queue extends Component {
  displayName = Queue.name

  constructor(props) {
    super(props);
    this.state = {
      queue: [],
      showCreate: false,
      showEdit: false
    };

    this.handleEditClose = this.handleEditClose.bind(this);
    this.handleEditShow = this.handleEditShow.bind(this);

    this.load();
  }

  async load() {
    const response = await fetch('QueueInformations');
    const queueInformations = await response.json();
    this.setState({ queue: queueInformations });
  }

  async addNewQueue(queueInformation) {
    var result = await fetch('QueueInformations', {
      method: 'POST',
      body: JSON.stringify(queueInformation),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    });

    if (result.ok) {
      var newQueue = await result.json();
      this.setState({ queue: this.state.queue.concat([newQueue]) });
    }
  }

  async handleEditClose(event, newQueue) {
    if (newQueue) {
      await this.addNewQueue(newQueue);
    }

    this.setState({ showEdit: false });
  }

  handleEditShow() {
    this.setState({ showEdit: true });
  }

  async handleChangeIsActive(event, queue, state) {
    const url = state
      ? `QueueInformations/${queue.id}/Activate`
      : `QueueInformations/${queue.id}/Deactivate`;

    const result = await fetch(url);

    if (result.ok) {
      queue.isActive = state;
      this.setState({ queue: this.state.queue });
    }
  }

  render() {
    const queueInformation = this.state.queue;
    console.log(queueInformation);

    return (
      <div>
        <h1>Gestion des queues connectées au pinpon</h1>
        <Table striped bordered condensed hover>
          <thead>
            <tr>
              <th>#</th>
              <th>Nom de la connexion</th>
              <th>Topic</th>
              <th>Subscription</th>
              <th>Status</th>
              <th>
                <Button onClick={this.handleEditShow}>
                  <Glyphicon glyph="plus" />
                  Ajouter une nouvelle queue
                </Button>
                <QueueModal show={this.state.showEdit} handleClose={this.handleEditClose} />
              </th>
            </tr>
          </thead>
          <tbody>
            {
              queueInformation.map(q =>
                (<tr key={q.id}>
                  <td>{q.id}</td>
                  <td>{q.displayName}</td>
                  <td>{q.topic}</td>
                  <td>{q.subscription}</td>
                  <td>
                    <Glyphicon glyph={q.isActive ? 'ok' : 'remove'} />
                    {q.isActive}
                  </td>
                  <td>
                    {q.isActive
                      ? <Button bsStyle="danger" onClick={(e) => this.handleChangeIsActive(e, q, false)}>
                        <Glyphicon glyph="ban-circle" />
                        Désactiver
                      </Button>
                      : <Button bsStyle="info" onClick={(e) => this.handleChangeIsActive(e, q, true)}>
                        <Glyphicon glyph="ok-circle" />
                        Activer
                      </Button>
                    }
                  </td>
                </tr>))
            }
          </tbody>
        </Table>
      </div>
    );
  }
}
