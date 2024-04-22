# Portfolio_HIM
게임명 : HIM
장르 : 1인칭 스릴러 탈출게임
플랫폼 : PC
제작의도 : 아웃라스트, 바이오하자드같은 1인칭 스릴러 게임을 생각하고 제작하였습니다. 탈출 오브젝트를 찾고 적을 피해 달아나는 스릴감을 느낄 수 있는 게임
특징 : 좁고 어두운 맵을 활용하여 손전등과 시야로 레벨디자인을 하였습니다.
한 줄 요약 : 적을 피해 탈출하는 1인칭 스릴러 게임

3D 1인칭 공포게임​

진행
타이틀 -> [게임시작, 설정, 나가기] -> [게임 시작] -> 탈출 오브젝트 찾기(열쇠) -> 탈출 지역으로 이동

컨트롤 - 상하좌우 움직이기, 앉기, 달리기, 좌,우 기울이기
NavMeshAgent로 이동하여 원치않는 장소로 이동하거나 캐릭터 끼임 방지

비동기로드로 로딩씬을 만들어서 랜더링이 끝나면 인게임 씬으로 이동.

사용된 기술

오쿨루젼 컬링
3D오브젝트가 많이 사용된 만큼 오쿨루전 컬링을 사용하여 성능 향상

적 AI
레벨에 지정된 웨이포인트로 FSM에 따라 움직임.
오직 발소리로만 플레이어를 감지하던 적은 WayPoint를 구현하여 플레이어를 감지하지 않을 때는 WayPoint를 따라 맵을 이동하도록 구현.
Idle: 가까운 웨이포인트로 이동, 이동 후에는 노드와 연결된 노드(웨이포인트)로 랜덤 이동(막다른 길이 아닌이상 이전에 지나온 노드로 이동하지 않음)
trace : 플레이어의 발소리를 감지하여 추적
attack : 감지한 플레이어가 공격 거리 이내에 들어올 시 공격, 공격은 유닛간의 거리와 각도를 계산하여 공격 판별

3D Sounds
3D사운드로 거리에 따른 사운드 구분, 움직임 시 구 형태의 Collider로 적이 발소리를 감지하여 추적할 수 있음.

오브젝트 풀링
발소리 구체 오브젝트가 과하게 생성, 삭제되어 성능을 저하시키지 않도록 하기 위함

레벨 디자인
좁은 맵과 쉬운 난이도를 높이기 위해 맵 전체 밝기를 더욱 어둡게하여 좁은 맵에서 손전등의 의존도를 높이고 배터리 소모량을 늘려 레벨디자인
