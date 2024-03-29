    CREATE TABLE IF NOT EXISTS analytics_conversations(
        MediaType VARCHAR(255) NOT NULL,
        QueueId VARCHAR(255) NULL,
        AgentId VARCHAR(255) NULL,
        DateFrom DATE NOT NULL,
        Offered int NOT NULL,
        Abandon int NOT NULL,
        tAbandon int NULL,
        Handle int NOT NULL,
        tHandle int NULL,
        Transferred int NOT NULL,
        Alert int NOT NULL,
        tAlert int NULL,
        Acw int NOT NULL,
        tAcw int NULL,
        Acd int NOT NULL,
        tAcd int NULL,
        Held int NOT NULL,
        tHeld int NULL,
        Talk int NOT NULL,
        tTalk int NULL,
        Id INT AUTO_INCREMENT PRIMARY KEY
    ) ENGINE=INNODB;