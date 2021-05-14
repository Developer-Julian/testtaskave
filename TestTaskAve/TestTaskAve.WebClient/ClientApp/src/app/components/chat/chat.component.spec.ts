import { NO_ERRORS_SCHEMA } from "@angular/core";
import { ChatComponent } from "./chat.component";
import { ComponentFixture, TestBed } from "@angular/core/testing";

describe("ChatComponent", () => {

  let fixture: ComponentFixture<ChatComponent>;
  let component: ChatComponent;
  beforeEach(() => {
    TestBed.configureTestingModule({
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
      ],
      declarations: [ChatComponent]
    });

    fixture = TestBed.createComponent(ChatComponent);
    component = fixture.componentInstance;

  });

  it("should be able to create component instance", () => {
    expect(component).toBeDefined();
  });
  
});
