import React, { Component } from 'react';
import { Modal, Button, FormGroup, FormControl, HelpBlock, ControlLabel, Glyphicon } from 'react-bootstrap';

export default class QueueModal extends Component {
  displayName = QueueModal.name

  constructor(props) {
    super(props);
    this.state = {
      displayName: '',
      connectionString: '',
      topic: '',
      subscription: ''
    };

    this.handleChange = this.handleChange.bind(this);
  }

  getValidationState(state, propertyName) {
    const length = state[propertyName].length;
    if (length > 5) return 'success';
    else if (length > 3) return 'warning';
    else if (length > 0) return 'error';

    return null;
  }

  handleChange(e) {
    this.setState({ [e.target.id]: e.target.value });
  }

  render() {
    const show = this.props.show;
    const handleClose = this.props.handleClose;

    const displayName = this.state.displayName || this.props.displayName;
    const connectionString = this.state.connectionString || this.props.connectionString;
    const topic = this.state.topic || this.props.topic;
    const subscription = this.state.subscription || this.props.subscription;

    const displayNameState = this.getValidationState(this.state, 'displayName');
    const connectionStringState = this.getValidationState(this.state, 'connectionString');
    const topicState = this.getValidationState(this.state, 'topic');
    const subscriptionState = this.getValidationState(this.state, 'subscription');

    return (
      <Modal show={show} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Ajouter une nouvelle queue à écouter</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <form>
            <FormGroup
              controlId="displayName"
              validationState={displayNameState}>
              <ControlLabel>Nom de la connexion</ControlLabel>
              <FormControl
                type="text"
                value={displayName}
                placeholder="Queue sur le compte azure de toto"
                onChange={this.handleChange} />
              <FormControl.Feedback />
              {displayNameState === 'error' && <HelpBlock>Le nom doit faire au moins 5 caractères</HelpBlock>}
            </FormGroup>

            <FormGroup
              controlId="connectionString"
              validationState={this.getValidationState(this.state, "connectionString")}>
              <ControlLabel>Chaîne de connexion</ControlLabel>
              <FormControl
                type="text"
                value={connectionString}
                placeholder="Endpoint=sb://pinpon.servicebus.windows.net/;SharedAccessKeyName=build-consumer;SharedAccessKey=..."
                onChange={this.handleChange} />
              <FormControl.Feedback />
              {connectionStringState === 'error' && <HelpBlock>La chaine de connexion n'est pas valide</HelpBlock>}
            </FormGroup>

            <FormGroup
              controlId="topic"
              validationState={this.getValidationState(this.state, "topic")}>
              <ControlLabel>Topic de la queue</ControlLabel>
              <FormControl
                type="text"
                value={topic}
                placeholder="build"
                onChange={this.handleChange} />
              <FormControl.Feedback />
              {topicState === 'error' && <HelpBlock>Le topic doit faire au moins 5 caractères</HelpBlock>}
            </FormGroup>

            <FormGroup
              controlId="subscription"
              validationState={this.getValidationState(this.state, "subscription")}>
              <ControlLabel>Subscription de la queue</ControlLabel>
              <FormControl
                type="text"
                value={subscription}
                placeholder="pinpon"
                onChange={this.handleChange} />
              <FormControl.Feedback />
              {subscriptionState === 'error' && <HelpBlock>La subscription doit faire au moins 5 caractères</HelpBlock>}
            </FormGroup>

          </form>

        </Modal.Body>
        <Modal.Footer>
          <Button onClick={e => handleClose(e, { displayName, connectionString, topic, subscription } )} bsStyle="success">
            <Glyphicon glyph="ok" />
            Ajouter la queue
          </Button>
        </Modal.Footer>
      </Modal>
    );
  }

}
