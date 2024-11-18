declare global {
  namespace Cypress {
    interface Chainable {
      login(hostOrIp: string, userId:string,password: string):Chainable<void>;
      logout():Chainable<void>;
    }
  }
}

Cypress.Commands.add('login', (hostOrIp,email, password) => {
    cy.visit('http://localhost:4200/login');
    cy.get('#ipOrhostName').type(hostOrIp);
    cy.get('#userName').type(email);
    cy.get('#password').type(password);
    cy.get('.btn-primary').click();
});

Cypress.Commands.add('logout', () => {
    cy.get('.btn').should('exist');
    cy.get('.btn').click();
});

export function getBaseUrl(){
  return Cypress.env('baseUrl');
}

