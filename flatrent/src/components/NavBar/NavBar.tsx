import React, { Component } from "react";
import {
  RouteComponentProps,
  withRouter,
} from "react-router-dom";
import LinkWithHighlight from "./LinkWithHighlight";
import "./NavBar.css";

class NavBar extends Component<{} & RouteComponentProps> {
  constructor(props: Readonly<RouteComponentProps & {}>) {
    super(props);
  }

  public render() {
    return (
      <div className="top-navbar">
        <nav>
          <ul>
            <li className="top-navbar__item top-navbar__name">
              Flat Rent Systems
            </li>
            <LinkWithHighlight
              link="/"
              currentUrl={this.props.location.pathname}
            >
              Pradžia
            </LinkWithHighlight>
            <LinkWithHighlight
              link="/flats"
              currentUrl={this.props.location.pathname}
            >
              Butai
            </LinkWithHighlight>
            <LinkWithHighlight
              link="/profile"
              currentUrl={this.props.location.pathname}
            >
              Paskyra
            </LinkWithHighlight>
            <LinkWithHighlight
              link="/logout"
              currentUrl={this.props.location.pathname}
            >
              Atsijungti
            </LinkWithHighlight>
          </ul>
        </nav>
      </div>
    );
  }
}

export default withRouter(NavBar);
