import React, { Component } from "react";
import { Header, Icon, List } from "semantic-ui-react";
import axios from "axios";
import "./App.css";

class App extends Component {
  state = { values: [] };

  async componentDidMount() {
    let { data: values } = await axios.get(
      "https://localhost:44307/api/values"
    );
    this.setState({ values });
  }

  render() {
    return (
      <div className="">
        <Header as="h2">
          <Icon name="users" />
          <Header.Content>Reactivities</Header.Content>
        </Header>
        <List>
          {this.state.values.map((value: any) => (
            <List.Item key={value.id}>{value.name}</List.Item>
          ))}
        </List>
      </div>
    );
  }
}

export default App;
